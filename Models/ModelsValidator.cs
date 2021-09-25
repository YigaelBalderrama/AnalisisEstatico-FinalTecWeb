using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SimpsonApp.Models
{
    public class ModelsValidator
    {
        public ModelsValidator() { }
        public List<string> validateModelFields(ModelStateDictionary modelState, bool updateMode = false)
        {
            var errorsListByField = new List<string>();
            if (!modelState.IsValid)
            {
                foreach (var field in modelState)
                {
                    if (field.Value.Errors.Count != 0)
                    {
                        var formatedErrorsMessage = getErrorsMessageOfAField(field, updateMode);
                        if (formatedErrorsMessage != "")
                            errorsListByField.Add($"{field.Key} -> ({formatedErrorsMessage})");
                    }
                }
            }
            return errorsListByField;
        }
        public string getErrorsMessageOfAField(KeyValuePair<string, ModelStateEntry> fieldErrors, bool updateMode = false)
        {
            string message = "";
            foreach (var err in fieldErrors.Value.Errors)
            {
                if (!(err.ErrorMessage == "Required" && updateMode))
                {
                    message = String.Concat(message, err.ErrorMessage, ", ");
                }
            }
            return message;
        }
    }
}
