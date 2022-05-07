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
2.5.5. func
2.5.6. call
2.5.7. rnd
2.6. Example Files

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

--- 2.5. Functions ---
:c incorporates some baseline functions to allow system flow. 
It should be noted that each function returns no value, and is therefore not usable in an expression. 
Functions must be the first token in a statement, and any inputs outside their scope will cause an error.

For the following function definitions, square brackets ([ ]) are used to denote the type of input expected in an area. These brackets would not be used in a real system.

--- 2.5.1. print ---

print([expression]);

The print function takes in an expression and displays its contents on a new line in the console.
The expression in question may also consist of a single token. Variables may also be used as a single token or as a component of the expression.

--- 2.5.2. input ---

input([variable name]);

The input function takes an input and stores the contents in the variable supplied. 
Note that the input will attempt to transform into the type of the variable, and therefore invalid inputs (such as 'a' into an integer variable) will cause an error to occur

--- 2.5.3. if ---

if([condition expression])
{
	[code]
};

The if statement function will run the code encapsulated within the { } brackets if the conditional expression returns true.
Note that, like the print function, the conditional expression may be a single token and may also include variables - but note that any inputs that do not result in a boolean output will cause errors.
The positioning of the { } brackets do not matter aside from the fact they must occur after the conditional - it is possible to have an if statement consist of a single line.
In addition to this, the indentation of the code block does not matter. However, the encapsulated code must be split into statements using the semicolon character as usual.

Finally, it should be noted that the if statement must still end with the semicolon character.

--- 2.5.4. while ---

while([condition expression])
{
	[code]
};

Much like the prior if statement, the while statement evaluates a condition and executes a block of code if true. 
However, when reaching the end of the block, the condition will be reevaluated and the code block will be repeated as long as the condition is true. If false, the next statement after the while clause will be executed.

--- 2.5.5. func ---

func([function name])
{
	[code]
};

The func statement defines a new function corresponding to the provided function name and stores it in global memory. 
The contents of this function can then be executed using the call command, described below.
It should be noted that functions are incapable of returning values and are assigned as they are parsed - meaning attempting to use a function before it has been declared will cause problems.

Much like variables, function names cannot share the name of a keyword or variable. This same principle applies to variables - in that they may not share a name with a defined function.

--- 2.5.6. call ---

call([function name]);

The call command will execute the contents of a previously defined function. 
As mentioned in the func segment, a function will not return any output, and therefore the call function cannot be used in an expression or assignment.
To use functions to assign values, variables can be accessed from inside a function when declared. 

--- 2.5.7. rnd ---

rnd([variable name]);

The rnd command will generate a random decimal value between 0 and 1 and assign said value into a specified variable.
Due to the typing of this, errors will occur if the variable specified is not of a decimal or string type.

--- 2.6. Example Files ---

Inside the supplied documents exists a folder called "ExampleFiles". This folder contains the five examples of the programming language - each seperated by subfolders specifying their type.
Inside these subfolders there is a single CodeFile.txt file for each function. The contents of this CodeFile.txt can replace the CodeFile.txt file in the directory of interpreter.exe to change what software is being run.
In order of simplicity, the following example files exist:

Mixed -
	A variety of different simple expressions and methods demonstrating the core concepts of the interpreter. 
	
Movement -
	A simple top down movement system, inwhich a player is able to move around a 3x3 grid by using WASD inputs. 
	
BoardGame -
	A two player board game inwhich each player attempts to travel a number of spaces before the other. 
	Each predicts a value they will roll above (from 0 to 5) and then rolls a dice. If they roll above their expected number they will move the number on the dice plus their input.
	If a player rolls below their expected value they will not move.
	The first player to reach the user defined goal wins the game.
	
AIBlackjack -
	A one-player blackjack game inwhich the player competes against a basic AI. 
	It should be noted that the deck in this software is unlimited, therefore more than four of each card value can exist at once.
	Additionally, in this software the ace is counted as a 1. When placed down this value is shown as an "A".
	10s, Jacks, Queens and Kings have the same value of 10. Each is shown with the first letter of their spelled name - "T" for 10, "J" for jack, "Q" for queen, "K" for king.


----- 100505349 University of Derby -----