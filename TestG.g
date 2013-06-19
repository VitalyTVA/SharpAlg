grammar TestG;

options {
  language = CSharp3;
}


testg 	:	expr EOF;
expr 	:	expr1 (PLUS expr1)?; 
expr1 	:	term (MULT term)?; 
term	:	INT | (LBRACE expr RBRACE);

ID  :	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;

INT :	'0'..'9'+
    ;

WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) {$channel=HIDDEN;}
    ;
 PLUS:	'+';
 MULT:	'*';
LBRACE:	'(';
RBRACE:	')';