using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;
/// <summary>  
/// Date time model binder.  
/// </summary>  
/// <seealso cref="IModelBinder" />  
public class DateTimeBinder
    : IModelBinder
{
    /// <summary>  
    /// The user culture  
    /// </summary>  
    protected readonly UserCultureInfo UserCulture;
    /// <summary>  
    /// Initializes a new instance of the <see cref="DateTimeBinder"/> class.  
    /// </summary>  
    /// <param name="userCulture">The user culture.</param>  
    public DateTimeBinder(UserCultureInfo userCulture)
    {
        UserCulture = userCulture;
    }
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }
        var valueProviderResult = bindingContext.ValueProvider
          .GetValue(bindingContext.ModelName);
        if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
        {
            return null;
        }
        DateTime datetime;
        if (DateTime.TryParse(valueProviderResult.FirstValue, null, DateTimeStyles.AdjustToUniversal, out datetime))
        {
            bindingContext.Result =
                ModelBindingResult.Success(UserCulture.GetUtcTime(datetime));
        }
        else
        {
            // TODO: [Enhancement] Could be implemented in better way.  
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                bindingContext.ModelMetadata
                .ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                  valueProviderResult.ToString(), nameof(DateTime)));
        }
        return Task.CompletedTask;
    }
}