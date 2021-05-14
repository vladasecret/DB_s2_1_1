﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FloatingDecimalBinderModel
{
    public class CustomFloatingModelBinder : IModelBinder
    {
        private readonly IModelBinder FallbackModelBinder;
        private string FieldName;
        private string FieldValueAsString;
        private string FieldValueAsNormalString;
        private ValueProviderResult PartValues;

        protected readonly Regex FloatPattern = new Regex(@"^(-?)[0-9]*(?:[.,][0-9]*)?$", RegexOptions.Compiled);
        protected readonly Regex FloatSeparator = new Regex(@"[.,]", RegexOptions.Compiled);

        public CustomFloatingModelBinder(IModelBinder fallback_model_binder)
        {
            this.FallbackModelBinder = fallback_model_binder;
        }

        public Task BindModelAsync(ModelBindingContext model_binding_context)
        {
            if (model_binding_context == null)
                throw new ArgumentNullException(nameof(model_binding_context));

            FieldName = model_binding_context.FieldName;

            PartValues = model_binding_context.ValueProvider.GetValue(FieldName);

            if (PartValues == ValueProviderResult.None)
                return FallbackModelBinder.BindModelAsync(model_binding_context);

            FieldValueAsString = FieldValueAsNormalString = PartValues.FirstValue;

            ////////////////////////////////////////////////////
            // если строка содержит десятичный/дробный разделитель -> строка дополняется нулями с обоих сторон.
            // Таким образом исключаем точки в начале или в конце строки:
            // ".5" -> "0.50"
            // "5." -> "05.0"
            if (FloatSeparator.IsMatch(FieldValueAsNormalString))
                FieldValueAsNormalString = "0" + FieldValueAsNormalString + "0";

            ////////////////////////////////////////////////////
            // заменим дробный разделитель на текущий системный
            FieldValueAsNormalString = FloatSeparator.Replace(FieldValueAsNormalString, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            ////////////////////////////////////////////////////
            // если не корректная строка -> передаём привязку системному привязчику
            if (!FloatPattern.IsMatch(FieldValueAsNormalString))
                return FallbackModelBinder.BindModelAsync(model_binding_context);

            if (model_binding_context.ModelMetadata.ModelType == typeof(double))
                model_binding_context.Result = ModelBindingResult.Success(GetDoubleFromString);
            else if (model_binding_context.ModelMetadata.ModelType == typeof(float))
                model_binding_context.Result = ModelBindingResult.Success(GetFloatFromString);
            else if (model_binding_context.ModelMetadata.ModelType == typeof(decimal))
                model_binding_context.Result = ModelBindingResult.Success(GetDecimalFromString);
            else
                return FallbackModelBinder.BindModelAsync(model_binding_context);

            return Task.CompletedTask;
        }

        public double GetDoubleFromString
        {
            get
            {
                if (!double.TryParse(FieldValueAsNormalString, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out double ret_val))
                    return 0;

                return ret_val;
            }
        }

        public decimal GetDecimalFromString
        {
            get
            {
                if (!decimal.TryParse(FieldValueAsNormalString, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal ret_val))
                    return 0;

                return ret_val;
            }
        }

        public float GetFloatFromString
        {
            get
            {
                if (!float.TryParse(FieldValueAsNormalString, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out float ret_val))
                    return 0;

                return ret_val;
            }
        }
    }
}
