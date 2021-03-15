using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

public enum TokenType 
{
    Set, If, ElseIf, Else, Symbol, Value,
    String, Float, Int, Bool,
    IsNot, Is, Operator,
    WhiteSpace, Text
}

public class Token
{
    public TokenType type;
    public string content;
    public List<Token> contents;

    public Token(TokenType type, string content)
    {
        this.type = type;
        this.content = content;
    }

    public Token(TokenType type, List<Token> contents)
    {
        this.type = type;
        this.contents = contents;
    }

    public override string ToString()
    {
        if (contents == null)
            return $"T({type.ToString()}, \"{Regex.Escape(content)}\")";
        else
        {
            List<string> stringContents = new List<string>();
            foreach (var token in contents)
                stringContents.Add(token.ToString());
            return $"T({type.ToString()}, [\n{String.Join("\n", stringContents)}\n])";
        }
    }
}

public class TokenDef
{
    public TokenType type;
    public Regex rx;

    public TokenDef(TokenType type, string pattern)
    {
        this.type = type;
        this.rx = new Regex($@"\G{pattern}");
    }
}

public class PassageLexer
{
    static List<TokenDef> tokenDefs = new List<TokenDef>
    {
        new TokenDef(TokenType.Set, @"\(set:\s*\$(?<symbol>[a-zA-Z0-9_-]+) to (?<value>.+?)\)"),
        new TokenDef(TokenType.If, @"\(if:\s*(?<condition>.+?)\)\s*\[\s*(?<body>.+?)\s*\]"),
        new TokenDef(TokenType.ElseIf, @"\(else-if:\s*(?<condition>.+?)\)\s*\[\s*(?<body>.+?)\s*\]"),
        new TokenDef(TokenType.Else, @"\(else:\s*\)\s*\[\s*(?<body>.+?)\s*\]"),
        new TokenDef(TokenType.Symbol, @"\$(?<symbol>[a-zA-Z0-0_-]+)"),
        new TokenDef(TokenType.String, @"""(?<value>.+)"""),
        new TokenDef(TokenType.Float, @"[-+]?\d*\.\d+([eE][-+]?\d+)?"),
        new TokenDef(TokenType.Int, @"[-+]?\d+"),
        new TokenDef(TokenType.Bool, @"(true|false)"),
        new TokenDef(TokenType.IsNot, @"is not"),
        new TokenDef(TokenType.Is, @"is"),
        new TokenDef(TokenType.Operator, @"[-+*/><]=?"),
        new TokenDef(TokenType.WhiteSpace, @"\s"),
        new TokenDef(TokenType.Text, @"."),
    };

    public static IEnumerable<Token> Tokenize(string text)
    {
        int cursor = 0;
        while (cursor < text.Length)
        {
            foreach (TokenDef tokenDef in tokenDefs)
            {
                Match match = tokenDef.rx.Match(text, startat: cursor);
                if(match.Success)
                {
                    switch(tokenDef.type)
                    {
                        case TokenType.Set:
                            string setSymbol = match.Groups["symbol"].Value; 
                            string setValue = match.Groups["value"].Value;
                            yield return new Token
                            (
                                tokenDef.type, new List<Token>
                                {
                                    new Token(TokenType.Symbol, setSymbol),
                                    new Token(TokenType.Value, Tokenize(setValue).ToList()),
                                }
                            );
                            break;
                        case TokenType.If:
                        case TokenType.ElseIf:
                            string ifCondition = match.Groups["condition"].Value; 
                            string ifBody = match.Groups["body"].Value;
                            yield return new Token
                            (
                                tokenDef.type, new List<Token>
                                {
                                    new Token(TokenType.Value, Tokenize(ifCondition).ToList()),
                                    new Token(TokenType.Value, Tokenize(ifBody).ToList()),
                                }
                            );
                            break;
                        case TokenType.Else:
                            string elseBody = match.Groups["body"].Value;
                            yield return new Token
                            (
                                tokenDef.type, new List<Token>
                                {
                                    new Token(TokenType.Value, Tokenize(elseBody).ToList()),
                                }
                            );
                            break;
                        case TokenType.Symbol:
                            string symbol = match.Groups["symbol"].Value;
                            yield return new Token(tokenDef.type, symbol);
                            break;
                        default:
                            yield return new Token(tokenDef.type, match.Value);
                            break;
                    }
                    cursor += match.Length;
                    break;
                }
            }
        }
    }
}
