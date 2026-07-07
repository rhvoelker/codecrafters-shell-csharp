grammar Command;

cmd : arg+ EOF ;
arg : UNQUOTED
    | SSTRING
    ;

SSTRING  : '\'' (D_SQUOTE | (~'\''))+ '\'' ;
UNQUOTED : (D_SQUOTE | (~[ \t\r\n'"]))+ ;
D_SQUOTE : '\'\'' ;
WS       : [ \t\r\n]+ -> skip ;