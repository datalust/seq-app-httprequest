// Copyright © Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using Seq.App.Http.Expressions;
using Seq.App.Http.Templates.Rendering;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Parsing;

namespace Seq.App.Http.Templates.Compilation
{
    class CompiledFormattedExpression : CompiledTemplate
    {
        readonly JsonValueFormatter _jsonFormatter;
        readonly Evaluatable _expression;
        readonly string? _format;
        readonly Alignment? _alignment;
        readonly IFormatProvider? _formatProvider;
        public CompiledFormattedExpression(Evaluatable expression, string? format, Alignment? alignment, IFormatProvider? formatProvider)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _format = format;
            _alignment = alignment;
            _formatProvider = formatProvider;
            _jsonFormatter = new JsonValueFormatter("$type");
        }

        public override void Evaluate(EvaluationContext ctx, TextWriter output)
        {
            if (_alignment == null)
            {
                EvaluateUnaligned(ctx, output, _formatProvider);
            }
            else
            {
                var writer = new StringWriter();
                EvaluateUnaligned(ctx, writer, _formatProvider);
                Padding.Apply(output, writer.ToString(), _alignment.Value);
            }
        }

        void EvaluateUnaligned(EvaluationContext ctx, TextWriter output, IFormatProvider? formatProvider)
        {
            var value = _expression(ctx);
            if (value == null)
                return; // Undefined is empty

            if (value is ScalarValue scalar)
            {
                if (scalar.Value is null)
                    return; // Null is empty

                if (scalar.Value is IFormattable fmt)
                    output.Write(fmt.ToString(_format, formatProvider));
                else
                    output.Write(scalar.Value.ToString());
            }
            else
            {
                _jsonFormatter.Format(value, output);
            }
        }
    }
}
