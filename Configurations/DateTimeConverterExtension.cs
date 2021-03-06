﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
/// <summary>  
/// <see cref="DateTimeConverter"/> initializer.  
/// </summary>  
public static class DateTimeConverterExtension
{
    /// <summary>  
    /// Registers the date time converter.  
    /// </summary>  
    /// <param name="option">The option.</param>  
    /// <param name="serviceCollection">The service collection.</param>  
    /// <returns></returns>  
    public static MvcJsonOptions RegisterDateTimeConverter(this MvcJsonOptions option,
      IServiceCollection serviceCollection)
    {
        // TODO: BuildServiceProvider could be optimized  
        option.SerializerSettings.Converters.Add(
            new DateTimeConverter(() => serviceCollection.BuildServiceProvider().GetService<UserCultureInfo>())
            );
        return option;
    }
}