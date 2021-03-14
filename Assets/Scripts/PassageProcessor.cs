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

    private object ParseType(string symbol)
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

    private object Eval(string expression)
    {
        if (stringRegex.IsMatch(expression))
        {
            return ParseType(expression);
        }
        else
        {
            var processedExpression = ProcessVariable(expression);
            System.Data.DataTable table = new System.Data.DataTable();
            var value = table.Compute(processedExpression, string.Empty);
            return table.Compute(processedExpression, string.Empty);
        }
    }

    private bool EvalCondition(string condition)
    {
        Regex isNotRegex = new Regex(@"(?<exp1>.+) is not (?<exp2>.+)");
        Match isNotMatch = isNotRegex.Match(condition);
        if(isNotMatch.Success)
        {
            GroupCollection groups = isNotMatch.Groups;
            var exp1 = Eval(groups["exp1"].Value);
            var exp2 = Eval(groups["exp2"].Value);
            return exp1 != exp2;
        }

        Regex isRegex = new Regex(@"(?<exp1>.+) is (?<exp2>.+)");
        Match isMatch = isRegex.Match(condition);
        if(isMatch.Success)
        {
            GroupCollection groups = isMatch.Groups;
            var exp1 = Eval(groups["exp1"].Value);
            var exp2 = Eval(groups["exp2"].Value);
            return exp1 == exp2;
        }

        return (bool)Eval(condition);
    }

    private string ProcessIfMacro(string originText)
    {
        Regex rx = new Regex(@"\(if:\s*(?<condition>.+?)\)\[(?<body>.+?)\]");
        MatchCollection matches = rx.Matches(originText);
        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;
            bool contidion = EvalCondition(groups["condition"].Value);
            if (contidion) {}
        }
        string setRemovedText = rx.Replace(originText, "").Trim();
        return setRemovedText;
    }

    private string ProcessSetMacro(string originText)
    {
        Regex rx = new Regex(@"\(set:\s*\$(?<key>[a-zA-Z0-9_-]+) to (?<value>.+?)\)");
        MatchCollection matches = rx.Matches(originText);
        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;
            variables[groups["key"].Value] = Eval(groups["value"].Value);
        }
        // For debuging
        foreach (var kv in variables)
        {
            Debug.Log($"{kv.Key}: {kv.Value} ({kv.Value.GetType()})");
        }
        string setRemovedText = rx.Replace(originText, "").Trim();
        return setRemovedText;
    }

    private string ProcessVariable(string originText)
    {
        Regex rx = new Regex(@"\$(?<varname>[a-zA-Z0-9_-]+)");
        MatchCollection matches = rx.Matches(originText);
        string varReplacedText = originText;
        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;
            var varname = groups["varname"].Value;
            varReplacedText = varReplacedText.Replace($"${varname}", variables[varname].ToString());
        }
        return varReplacedText;
    }

    private string ProcessPassageText(string rawText)
    {
        string setRemovedText = ProcessSetMacro(rawText);
        string varReplacedText = ProcessVariable(setRemovedText);
        return varReplacedText;
    }

    public static PassageProcessor Process(Passage passage, Dictionary<string, object> initialVariables)
    {
        var processor = new PassageProcessor(initialVariables);
        processor.passage.pid = passage.pid;
        processor.passage.name = passage.name;
        processor.passage.text = processor.ProcessPassageText(passage.text);
        processor.passage.links = passage.links; // TODO: process links
        return processor;
    }
}
