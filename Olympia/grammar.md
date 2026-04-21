<program> ::= <class_declaration>*
<class_declaration> ::= <access_modifer> "class" IDENTIFIER "{" <class_body> "}"
<class_body> ::= (<method_declaration> | <field_assignment_statement>)*
<method_declaration> ::= <access_modifier> <type> IDENTIFIER "(" <parameter_list> ")" "{" <statement>* "}"
<parameter_list> ::= <parameter> ("," <parameter>)* | ""
<parameter> ::= <type> IDENTIFIER
<statement> ::= <assignment_statement> | <field_assignment_statement>
<field_assignment_statement> ::= <access_modifier> <assignment_statement>
<assignment_statement> ::= <type> IDENTIFIER "=" <value> ";"
<type> ::= "int" | "float"
<value> ::= INT_LITERAL | FLOAT_LITERAL | IDENTIFIER
<access_modifier> ::= "public" | "private"