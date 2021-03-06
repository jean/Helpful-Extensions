﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;

namespace Piedone.HelpfulExtensions.Extensions.Tokens
{
    [OrchardFeature("Piedone.HelpfulExtensions.Tokens")]
    public class TextTokens : ITokenProvider
    {
        public Localizer T { get; set; }


        public TextTokens()
        {
            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            context.For("Text", T("Text"), T("Tokens for text strings"))
                .Token("WrapNotEmpty:*", T("WrapNotEmpty:<left wrapper>[,<right wrapper>]"), T("Wraps the text if it's not empty from left and optionally from right with the specified texts."));
        }

        public void Evaluate(EvaluateContext context)
        {
            context.For<String>("Text", () => "")
                .Token( // {WrapNotEmpty:<left wrapper>[,<right wrapper>]}
                    token =>
                    {
                        if (token.StartsWith("WrapNotEmpty:", StringComparison.OrdinalIgnoreCase))
                        {
                            return token.Substring("WrapNotEmpty:".Length);
                        }
                        return null;
                    },
                    (param, token) =>
                    {
                        if (String.IsNullOrEmpty(token)) return String.Empty;

                        var index = param.IndexOf(',');

                        // No right wrapper
                        if (index == -1)
                        {
                            token = param + token;
                        }
                        else
                        {
                            token = param.Substring(0, index) + token + param.Substring(index + 1);
                        }

                        return token;
                    });

        }
    }
}