﻿using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Core.Strings;

namespace Umbraco.Web.PropertyEditors.ParameterEditors
{
    [DataEditor(
        "tabPickerMultiple",
        EditorType.MacroParameter,
        "Multiple Tab Picker",
        "entitypicker")]
    public class MultiplePropertyGroupParameterEditor : DataEditor
    {
        public MultiplePropertyGroupParameterEditor(
            ILogger logger,
            IDataTypeService dataTypeService,
            ILocalizationService localizationService,
            ILocalizedTextService localizedTextService,
            IShortStringHelper shortStringHelper)
            : base(logger, dataTypeService, localizationService, localizedTextService, shortStringHelper)
        {
            // configure
            DefaultConfiguration.Add("multiple", true);
            DefaultConfiguration.Add("entityType", "PropertyGroup");
            //don't publish the id for a property group, publish its alias, which is actually just its lower cased name
            DefaultConfiguration.Add("publishBy", "alias");
        }
    }
}