using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
/// <summary>  
/// <see cref="DateTimeBinder"/> provider.  
/// </summary>  
/// <seealso cref="IModelBinderProvider" />  
public class DateTimeBinderProvider
    : IModelBinderProvider
{
    /// <summary>  
    /// The user culture  
    /// </summary>  
    protected readonly Func<UserCultureInfo> UserCulture;
    /// <summary>  
    /// Initializes a new instance of the <see cref="DateTimeBinderProvider"/> class.  
    /// </summary>  
    /// <param name="userCulture">The user culture.</param>  
    public DateTimeBinderProvider(Func<UserCultureInfo> userCulture)
    {
        UserCulture = userCulture;
    }
    /// <summary>  
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" /> based on <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.  
    /// </summary>  
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.</param>  
    /// <returns>  
    /// An <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />.  
    /// </returns>  
    /// <exception cref="System.ArgumentNullException">context</exception>  
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        if (context.Metadata.UnderlyingOrModelType == typeof(DateTime))
        {
            return new DateTimeBinder(UserCulture());
        }
        return null; // TODO: Find alternate.  
    }
}