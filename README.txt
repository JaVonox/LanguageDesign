----- :c Language README FILE -----

:c (pronounced "colon-c") is a simple interpreted language using a C# framework
Designed to implement the core essentials of a language, :c has a simple yet expandable structure
This file will discuss the basic usage of the interpreter and the features and syntax provided in the language

--- CONTENTS ---
1. Startup
2.1. Data Types
2.2. Operations
2.3. General Syntax
2.4. Variables
2.5. Functions
2.5.1. print
2.5.2. input
2.5.3. if
2.5.4. while

--- 1. Startup ---
To begin using the :c interpreter, first follow the installation steps outlined in BUILD.txt
when complete, take note of the CodeFile.txt file in the installed directory. This file contains the script that will be interpreted when running the interpreter.exe file.
If a CodeFile.txt file does not exist, create a new "CodeFile.txt" file in the same directory as interpreter.exe

After ensuring a non-empty CodeFile.txt exists in the aforementioned directory, running the interpreter.exe file will both parse and execute the script.
Example scripts can be found in the TestFiles folder in the same directory as this README file. To execute a script, simply copy the contents of a new script into the aforementioned CodeFile.txt file.

--- 2.1. Data Types ---
:c contains support for the following data types:
Integer,
Decimal,
String,
Boolean

Constant data (or rather, that which is not within a variable) is automatically assigned to the most appropriate data type. For example,
entering the value 10 will automatically designate said constant as an integer type, whereas 10.1 will be defined as a decimal type.
Similarly, the values true and false will automatically be categorised as a boolean value.
The exception to this is string values, which must be enclosed within " characters. While blank inputs ("") are permitted, trailing delimiters (""") will halt parsing.

Decimal and integer values are permitted to use negative values, however this is limited to a single negative symbol (--10 will not be accepted as a value).
Similarly, the use of multiple decimal points in a single value (1.2.5) will cause parsing to halt.

--- 2.2. Operations ---
The following operations are supported by the :c interpreter:
+ (Addition),
- (Subtraction),
* (Multiplication),
/ (Division),
< (Less than comparitor),
> (Greater than comparitor),
== (Equal to comparitor),
!= (Not equal to comparitor),
<= (Less than or equal to comparitor),
>= (Greater than or equal to comparitor),
&& (And logic gate),
|| (Or logic gate),
! (Not logic gate)

Operators can be used within any expression, and multiple operators can be used in the same equation and will be ordered as appropriate. To add to this, parenthesis - ( and ) - may be used to enclose a statement.
Most operators require two values to be used, incompatiable values (discussed further below) will cause errors to be reported. 
The exception to this is the not (!) operator, which requires only one value, placed to the right of it (such as !true)

In addition, some operations may mutate the data type of a value. This must be kept in mind when using variables, which will reject invalid types.
Each data type may use the following operations

Integer:
+ (Accepts integer, decimal and string. Returns integer if other value is integer. Returns decimal if the other value is decimal. Returns string if the other value is string, appending the integer to the end of the string set),
- (Accepts integer and decimal. Returns integer if other value is integer. Returns decimal if the other value is decimal),
* (Accepts integer and decimal. Returns integer if other value is integer. Returns decimal if the other value is decimal),
/ (Accepts integer and decimal. Always returns decimal result),
< (Accepts integer and decimal. Always returns boolean result),
> (Accepts integer and decimal. Always returns boolean result),
== (Accepts integer and decimal. Always returns boolean result),
<= (Accepts integer and decimal. Always returns boolean result),
>= (Accepts integer and decimal. Always returns boolean result)

Decimal:
+ (Accepts integer, decimal and string. Returns decimal if other value is integer or decimal. Returns string if the other value is string, appending the decimal value to the end of the string set),
- (Accepts integer and decimal. Always returns decimal result),
* (Accepts integer and decimal. Always returns decimal result),
/ (Accepts integer and decimal. Always returns decimal result),
< (Accepts integer and decimal. Always returns boolean result),
> (Accepts integer and decimal. Always returns boolean result),
== (Accepts integer and decimal. Always returns boolean result),
<= (Accepts integer and decimal. Always returns boolean result),
>= (Accepts integer and decimal. Always returns boolean result)

String:
+ (Accepts integer, decimal, string and boolean. Returns a string result with the other value appended to the end of the string),
== (Accepts string. Always returns boolean result)

Boolean:
== (Accepts boolean. Always returns boolean result),
&& (Accepts boolean. Always returns boolean result),
|| (Accepts boolean. Always returns boolean result),
! (Does not accept another value. Always returns boolean result)

--- 2.3 General Syntax ---
The :c language follows a C derivative structure. each statement is expected to end with a semicolon, else the next line is considered part of the same statement.
This principle extends to functions - including while and if statments. This principle is discussed further in the following segments.

--- 2.4. Variables ---
Variables may take the form of any of the aforementioned data types. Variables are typed statically, and attempts to assign an invalid value to a variable will cause errors.
All variables are globally scoped and do not accept redefinition. This is especially of note when using loops, as variables declared within a loop may cause errors due to redefinition.

Variable names have strict guidelines - they may only consist of alphanumeric characters (with the exception of the _ character). 
Furthermore the first character of a variable name cannot be numeric - nor can the entire set consist entirely of numeric characters.
Finally, variable names may not take the form of an existing function or variable name.

Variable declaration takes a consist syntax - inwhich a variable type is defined first, followed by the identifier, followed by a semicolon (as an example - int a; - would be a valid integer value declaration).
The following keywords can be used to define a variable:
int (Integer),
float (Decimal),
string (String),
bool (Boolean)

Two things should be noted. First and foremost, the variable typing keyword is case sensitive - inputs like "Int" will not be accepted.
Secondly, a variable declaration cannot be used with an assignment. Assigning a value to a variable must occur as a seperate statement.
This means inputs like "int a = 10;" are not accepted. Instead, the correct format would be "int a; a = 10;"

As noted, the = operator can be used on any variable identifier (provided it has been declared) to assign a value.
This assignment may also take the form of an equation, such as "a = 10 + 5;". 
In addition, other declared variables (given that they are of a valid type) may be used as a component of an assignment, allowing for assignments like "a = b + 10;".
Provided a variable has been given a value, it may also be used in the assignment of itself - such as "a = a + 10;"

An = operator may not be used in any situation outside the scope of assignment - assignment composes a single statement that expects an expression, and therefore does not return any value.
In addition to to this, multiple = operators may not be used in a single operation (with the exception of the == operation, which is distinct from the = assignment operator).
Failure to adhere to these rules will result in an error occuring.

--- 2.5. Functions ---
:c incorporates some baseline functions to allow system flow. 
It should be noted that each function returns no value, and is therefore not usable in an equation. 
Functions must be the first token in a statement, and any inputs outside their scope will cause an error.

The following functions are currently implemented:


----- 100505349 University of Derby -----