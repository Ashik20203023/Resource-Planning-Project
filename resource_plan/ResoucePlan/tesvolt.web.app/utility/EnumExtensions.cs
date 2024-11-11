using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesvolt.core.app
{
    public static class EnumExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });
        }
    }
}
