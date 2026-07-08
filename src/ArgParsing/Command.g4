grammar Command;

cmd : arg (WS arg)* EOF ;
arg : (UNQUOTED | SSTRING | DSTRING | D_SQUOTE | D_DQUOTE | ESCAPE)+ ;

ESCAPE   : '\\' . ;
SSTRING  : '\'' ('\\\'' | (~[\\']))+ '\'' ;
DSTRING  : '"' ('\\"' | (~[\\"]))+ '"' ;
UNQUOTED : (~[ \\\t\r\n'"])+ ;
D_SQUOTE : '\'\'' ;
D_DQUOTE : '""' ;
WS       : [ \t\r\n]+ ;