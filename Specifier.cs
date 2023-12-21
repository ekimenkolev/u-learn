// Вставьте сюда финальное содержимое файла Specifier.cs
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {   
            var pr = typeof(T).GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            if (pr == null)
                return null;
            else return pr.Description;
        }

        public string[] GetApiMethodNames()
        {
            return typeof(T).GetMethods()
                .Where(met => met.GetCustomAttributes<ApiMethodAttribute>().Any())
                .Select(met => met.Name)
                .ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var met = typeof(T).GetMethod(methodName);
            if (met == null)
                return null;
            var pr = met.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            if (pr == null)
                return null;
			if (!met.GetCustomAttributes<ApiMethodAttribute>().Any())
                return null;
            else return pr.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var met = typeof(T).GetMethod(methodName);
            if (met == null)
                return null;
            if (!met.GetCustomAttributes<ApiMethodAttribute>().Any())
                return null;
            var parametr = met.GetParameters().Select(param => param.Name);
            return parametr.ToArray();
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var met = typeof(T).GetMethod(methodName);
            if (met == null)
                return null;
            if (!met.GetCustomAttributes<ApiMethodAttribute>().Any())
                return null;
            var parametr = met.GetParameters().Where(param => param.Name == paramName);
            if (!parametr.Any())
                return null;
            var pr = parametr.First().GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            if (pr == null)
                return null;
            else return pr.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var met = typeof(T).GetMethod(methodName);
            var param1 = new ApiParamDescription
            {
                ParamDescription = new CommonDescription(paramName)
            };
            if (met == null)
                return param1;
            if (!met.GetCustomAttributes<ApiMethodAttribute>().Any())
                return param1;
            var parametr = met.GetParameters().Where(param => param.Name == paramName);
            if (!parametr.Any())
                return param1;
            var vl = parametr.First().GetCustomAttributes<ApiIntValidationAttribute>().FirstOrDefault();
            if (vl != null)
            {
                param1.MaxValue = vl.MaxValue;
                param1.MinValue = vl.MinValue;
            }
            var pr2 = parametr.First().GetCustomAttributes<ApiRequiredAttribute>().FirstOrDefault();
            if (pr2 != null)
            {
                param1.Required = pr2.Required;
            }
            var pr1 = parametr.First().GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            if (pr1 != null)
            {
                param1.ParamDescription.Description = pr1.Description;
            }
            return param1;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
		
            var met = typeof(T).GetMethod(methodName);
            if (met == null)
                return null;
            if (!met.GetCustomAttributes<ApiMethodAttribute>().Any())
                return null;
            var p3 = GetApiMethodParamNames(methodName).
                Select(param => GetApiMethodParamFullDescription(methodName, param));
            var met3 = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(methodName, 
                GetApiMethodDescription(methodName)),
                ParamDescriptions = GetApiMethodParamNames(methodName).
                Select(parametr => GetApiMethodParamFullDescription(methodName, parametr)).ToArray()
            };
            var param1 = new ApiParamDescription
            {
                ParamDescription = new CommonDescription()
            };
            var rpar = met.ReturnParameter;
            var vl = rpar.
                GetCustomAttributes<ApiIntValidationAttribute>().FirstOrDefault();
				var setpar = false;
            if (vl != null)
            {
                setpar = true;
                param1.MaxValue = vl.MaxValue;
                param1.MinValue = vl.MinValue;
            }
			var pr1 = rpar.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault();
            if (pr1 != null)
            {
                setpar = true;
                param1.ParamDescription.Description = pr1.Description;
            }
            var pr2 = rpar.
                GetCustomAttributes<ApiRequiredAttribute>().FirstOrDefault();
				
            if (pr2 != null)
            {
                setpar = true;
                param1.Required = pr2.Required;
            }
            if (setpar)
            {
                met3.ReturnDescription = param1;
            }
            return met3;
        }
    }
}
