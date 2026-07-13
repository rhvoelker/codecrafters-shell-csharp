grammar Command;

cmd        : arg (WS+ arg)* (WS+ red)* EOF ;
arg        : str+ ;
red        : red_stream? red_op WS* arg ;
red_stream : NUMBER+ ;
red_op     : REDIRECT REDIRECT? ;
str        : SLASH .                                             #EscapeCharacter
           | (NON_WS | NUMBER)+                                  #UnquotedString
           | SQUOTE (sstr_inner)* SQUOTE                         #SingleQuotedString
           | DQUOTE (dstr_inner)* DQUOTE                         #DoubleQuotedString
           ;
sstr_inner : (NON_WS | WS | DQUOTE | SLASH | NUMBER | REDIRECT)+ #SingleStringText
           ;
dstr_inner : SLASH .                                             #DoubleStringEscapeCharacter
           | (NON_WS | WS | SQUOTE | NUMBER | REDIRECT)+         #DoubleStringText
           ;

WS       : [ \t\r\n] ;
SQUOTE   : '\'' ;
DQUOTE   : '"' ;
SLASH    : '\\' ;
NUMBER   : [0-9] ;
REDIRECT : '>' ;
NON_WS   : (~[ \t\r\n]) ;