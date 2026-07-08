grammar Command;

cmd : arg (WS arg)* EOF ;
arg : (UNQUOTED | SSTRING | DSTRING | D_SQUOTE | D_DQUOTE)+;

SSTRING  : '\'' (~'\'')+ '\'' ;
DSTRING  : '"' (~'"')+ '"' ;
UNQUOTED : (~[ \t\r\n'"])+ ;
D_SQUOTE : '\'\'' -> skip ;
D_DQUOTE : '""' -> skip ;
WS       : [ \t\r\n]+;