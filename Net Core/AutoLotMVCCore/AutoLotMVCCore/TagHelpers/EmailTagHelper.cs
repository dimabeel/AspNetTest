using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoLotMVCCore.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public string EmailName { get; set; }

        public string EmailDomain { get; set; }

        public override void Process(TagHelperContext context,
            TagHelperOutput output)
        {
            output.TagName = "a";
            var adress = $"{EmailName}@{EmailDomain}";
            output.Attributes.SetAttribute("href", $"mailto:{adress}");
            output.Content.SetContent(adress);
        }
    }
}
