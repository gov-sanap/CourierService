using CourierService.Contracts;
using CourierService.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CourierService.Models
{
    public class Rule
    {
        public string Key { get; set; }
        public OperatorType Operator { get; set; }
        public string Value { get; set; }

        public bool Qualify(Order order)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(Key))
                {
                    if(!GetValue(order, Key, out object fieldValue))
                    {
                        throw new Exception("Invalid Key");
                    }

                    switch (Operator)
                    {
                        case OperatorType.LessThan:
                            result = (double)fieldValue < double.Parse(Value);
                            break;
                        case OperatorType.GreaterThan:
                            result = (double)fieldValue > double.Parse(Value);
                            break;
                        case OperatorType.LessThanEqual:
                            result = (double)fieldValue <= double.Parse(Value);
                            break;
                        case OperatorType.GreaterThanEqual:
                            result = (double)fieldValue >= double.Parse(Value);
                            break;
                        case OperatorType.Equal:
                            result = fieldValue.ToString().Equals(Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Invalid Rule with Key: {Key ?? ""} Operator: {Operator.ToString() ?? ""} Value: {Value ?? ""} ");
                Console.WriteLine($"Got Exception: {ex.Message}");
            }
            return result;
        }

        private bool GetValue(Order order, string pathName, out object fieldValue)
        {
            object currentObject = order;
            string[] fieldNames = pathName.Split(".");

            foreach (string fieldName in fieldNames)
            {
                // Get type of current record 
                Type curentRecordType = currentObject.GetType();
                PropertyInfo property = curentRecordType.GetProperty(fieldName);

                if (property != null)
                {
                    currentObject = property.GetValue(currentObject, null);
                }
                else
                {
                    fieldValue = null;
                    return false;
                }
            }
            fieldValue = currentObject;
            return true;
        }
    }
}
