grammar Command;

cmd        : arg (WS+ arg)* EOF ;
arg        : str+ ;
str        : SLASH .                         #EscapeCharacter
           | NON_WS+                         #UnquotedString
           | SQUOTE (sstr_inner)* SQUOTE     #SingleQuotedString
           | DQUOTE (dstr_inner)* DQUOTE     #DoubleQuotedString
           ;
sstr_inner : (NON_WS | WS | DQUOTE | SLASH)+ #SingleStringText
           ;
dstr_inner : SLASH .                         #DoubleStringEscapeCharacter
           | (NON_WS | WS | SQUOTE)+         #DoubleStringText
           ;

WS : [ \t\r\n] ;
SQUOTE : '\'' ;
DQUOTE : '"' ;
SLASH : '\\' ;
NON_WS : (~[ \t\r\n]) ;