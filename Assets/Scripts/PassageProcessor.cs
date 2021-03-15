using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class PassageProcessor
{
    public Passage passage = new Passage();
    public Dictionary<string, object> variables;

    Regex stringRegex = new Regex(@"""(?<value>.+)""");
    Regex intRegex = new Regex(@"(?<value>\d+)");
    Regex floatRegex = new Regex(@"(?<value>[0-9.]+)");
    Regex boolRegex = new Regex(@"(?<value>[false|true]+)");

    public PassageProcessor(Dictionary<string, object> initialVariables)
    {
        variables = initialVariables;
    }

    private object Parse(string symbol)
    {        
        Match stringMatch = stringRegex.Match(symbol);
        if(stringMatch.Success)
            return stringMatch.Groups["value"].Value;

        Match intMatch = intRegex.Match(symbol);
        if(intMatch.Success)
            return int.Parse(intMatch.Groups["value"].Value);

        Match floatMatch = floatRegex.Match(symbol);
        if(floatMatch.Success)
            return float.Parse(floatMatch.Groups["value"].Value);

        Match boolMatch = boolRegex.Match(symbol);
        if(boolMatch.Success)
            return bool.Parse(boolMatch.Groups["value"].Value);
        
        throw new NotImplementedException($"Invalid variable value: {symbol}");
    }

    private string GetExpression(Token token)
    {
        switch (token.type)
        {
            case TokenType.Symbol:
                object value = variables[token.content];
                if (value is string)
                    return $"'{value}'";
                else
                    return value.ToString();
            case TokenType.Value:
                return Eval(token.contents).ToString();
            case TokenType.String:
                return $"'{Parse(token.content)}'";
            case TokenType.IsNot:
                return "<>";
            case TokenType.Is:
                return "=";
            default:
                return token.content;
        }
    }

    private object Eval(List<Token> tokens)
    {
        if (tokens.Count <= 1)
        {
            return Parse(tokens[0].content);
        }
        else
        {
            List<string> expressions = new List<string>();
            foreach (var token in tokens)
                expressions.Add(GetExpression(token));
            string expression = String.Join(" ", expressions);
            
            var table = new System.Data.DataTable();
            try
            {
                var value = table.Compute(expression, string.Empty);
                return value;
            }
            catch (SyntaxErrorException e)
            {
                Debug.Log($"-- expression: {expression}");
                throw e;
            }   
        }
    }

    private string ProcessTokens(IEnumerable<Token> tokens)
    {
        bool lastIf = false;
        StringBuilder result = new StringBuilder();
        foreach (var token in tokens)
        {
            switch (token.type)
            {
                case TokenType.Set:
                    string setSymbol = token.contents[0].content; 
                    object setValue = Eval(token.contents[1].contents);
                    variables[setSymbol] = setValue;
                    break;
                case TokenType.If:
                    bool ifCondition = (bool)Eval(token.contents[0].contents);
                    if (ifCondition)
                        result.Append(ProcessTokens(token.contents[1].contents));
                    lastIf = ifCondition;
                    break;
                case TokenType.ElseIf:
                    if (lastIf) continue;
                    bool elseIfCondition = (bool)Eval(token.contents[0].contents);
                    if (elseIfCondition)
                        result.Append(ProcessTokens(token.contents[1].contents));
                    lastIf = elseIfCondition;
                    break;
                case TokenType.Else:
                    if (lastIf) continue;
                    result.Append(ProcessTokens(token.contents[0].contents));
                    lastIf = false;
                    break;
                case TokenType.Symbol:
                    result.Append(variables[token.content].ToString());
                    break;
                default:
                    result.Append(token.content);
                    break;
            }
        }
        return result.ToString();
    }

    public static PassageProcessor Process(Passage passage, Dictionary<string, object> initialVariables)
    {
        var processor = new PassageProcessor(initialVariables);
        processor.passage.pid = passage.pid;
        processor.passage.name = passage.name;
        
        var tokens = PassageLexer.Tokenize(passage.text);
        processor.passage.text = processor.ProcessTokens(tokens);
        processor.passage.links = passage.links; // TODO: process links

        return processor;
    }
}
