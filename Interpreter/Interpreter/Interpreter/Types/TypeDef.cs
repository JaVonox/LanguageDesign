﻿using System;
using System.Collections.Generic;
using System.Text;
using Nodes;
using Tokens;
using Interpreter;
namespace TypeDef
{
    public class Item
    {
        private TypeTemplate content { get; set; }

        public dynamic ReturnDeepValue() //Return the true value of an item - variable contents or real value
        {
            if (GetType() == typeof(TypeTemplate.Identifier) && Interpreter.Interpreter.globalVars.VarContains(content.contents.ToString()))
            {
                return Interpreter.Interpreter.globalVars.VarGetItem(content.contents.ToString()).ReturnShallowValue(); //Get the contents of the item
            }
            else
            {
                return ReturnShallowValue(); //Get the non-variable item
            }
        }

        public Item ReturnDeepVar() //Returns the variable contained within a variable
        {
            if (GetType() == typeof(TypeTemplate.Identifier) && Interpreter.Interpreter.globalVars.VarContains(content.contents.ToString()))
            {
                return Interpreter.Interpreter.globalVars.VarGetItem(content.contents.ToString()); //Get the contents of the item
            }
            else
            {
                throw new Exception("Invalid variable");
            }
        }

        public Type ReturnDeepVarType()
        {
            if (GetType() == typeof(TypeTemplate.Identifier) && Interpreter.Interpreter.globalVars.VarContains(content.contents.ToString()))
            {
                return Interpreter.Interpreter.globalVars.VarGetItem(content.contents.ToString()).GetType(); //Get the contents of the item
            }
            else
            {
                return GetType();
            }
        }
        public dynamic ReturnShallowValue() //Return the type-adjusted contents of the item. In variables this stores the name of the value
        {
            return content.contents; //Get the non-variable item
        }

        public Type GetType() //Return the value type
        {
            return content.GetType();
        }

        public string GetStringContents()
        {
            return ReturnDeepValue().ToString();
        }
        //Constructors
        public Item(Token item)
        {
            Setter((NodeContentType)item.type, item.contents);
        }
        public Item(NodeContentType type, object inContents)
        {
            Setter(type, inContents);
        }
        private void Setter(NodeContentType type, object inContents)
        {
            switch (type)
            {
                case NodeContentType.Integer:
                    {
                        content = new TypeTemplate.Integer();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.Decimal:
                    {
                        content = new TypeTemplate.Decimal();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.String:
                    {
                        content = new TypeTemplate.String();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.Boolean:
                    {
                        content = new TypeTemplate.Boolean();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.Operation:
                    {
                        content = new TypeTemplate.Operation();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.End:
                    {
                        content = new TypeTemplate.End();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.Bracket:
                    {
                        content = new TypeTemplate.Bracket();
                        content.contents = inContents;
                        break;
                    }
                case NodeContentType.Identifier: //Variables and keywords
                    {
                        content = new TypeTemplate.Identifier();
                        content.contents = inContents; //contents is variable name
                        break;
                    }
                default:
                    throw new Exception("Unimplemented type: " + type + ":" + inContents);
            }
        }

        private static void SetVars(ref Item self, ref Item ext)
        {
            if(self.GetType() == typeof(TypeTemplate.Identifier))
            {
                GetVariable(ref self); //Sets self to its appropriate value if needed 
            }

            if (ext.GetType() == typeof(TypeTemplate.Identifier))
            {
                GetVariable(ref ext); //Sets ext to its appropriate value if needed 
            }
        }
        private static void GetVariable(ref Item item) //Gets the variable item of a value
        {
            if (item.GetType() == typeof(TypeTemplate.Identifier))
            {
                if (Interpreter.Interpreter.globalVars.VarContains(item.ReturnShallowValue()))
                {
                    item = Interpreter.Interpreter.globalVars.VarGetItem(item.ReturnShallowValue());
                }
                else
                {
                    throw new Exception("Use of variable " + item.ReturnShallowValue() + " before assignment");
                }

            }
        }
        //Equations

        public static Item operator +(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.AddOperation(self, ext);
        }
        public static Item operator -(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.SubOperation(self, ext);
        }
        public static Item operator *(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.MultOperation(self, ext);
        }
        public static Item operator /(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.DivOperation(self, ext);
        }

        //Value checking
        public static Item LessThan(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.LessThan(self, ext);
        }
        public static Item GreaterThan(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.GreaterThan(self, ext);
        }
        public static Item EqualTo(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.EqualTo(self, ext);
        }
        public static Item NotEqualTo(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.NotEqualTo(self, ext);
        }
        public static Item LessThanEqualTo(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            Item lessOp = self.content.LessThan(self, ext);
            if (lessOp.ReturnDeepValue()) { return lessOp; }
            Item equalOp = self.content.EqualTo(self, ext);
            return equalOp;
        }
        public static Item GreaterThanEqualTo(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            Item gOp = self.content.GreaterThan(self, ext);
            if (gOp.ReturnDeepValue()) { return gOp; }
            Item equalOp = self.content.EqualTo(self, ext);
            return equalOp;
        }

        //Boolean
        public static Item And(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.And(self, ext);
        }
        public static Item Or(Item self, Item ext)
        {
            SetVars(ref self, ref ext);
            return self.content.Or(self, ext);
        }
        public static Item Not(Item self)
        {
            GetVariable(ref self);
            return self.content.Not(self);
        }
        public static void SetContent(Item self, Item newContents)
        {
            if (self.GetType() == typeof(TypeTemplate.Identifier))
            {
                if (Interpreter.Interpreter.globalVars.VarContains(self.ReturnShallowValue()))
                {
                    if (self.ReturnDeepVar().content.CanAssign(newContents.ReturnDeepValue())) //Check if the variable types are compatible
                    {
                        Interpreter.Interpreter.globalVars.VarUpdateItem(self.ReturnShallowValue(), newContents.ReturnDeepValue()); //Set the value in the variable storage
                    }
                    else
                    {
                        throw new Exception("Value " + newContents.ReturnDeepValue() + " with type " + newContents.ReturnDeepVarType().Name + " cannot be assigned to variable " + self.ReturnShallowValue() + " of type " + self.ReturnDeepVarType().Name);
                    }
                }
                else
                {
                    throw new Exception("Use of variable " + self.ReturnShallowValue() + " before assignment");
                }
            }
            else
            {
                throw new Exception("Assignment operator on non-variable item");
            }
        }

        public static bool Assignable(Item self, string query) //Check if an input is of the correct type
        {
            if (self.ReturnDeepVar().content.IsValidValue(query)) //Check if the types are compatible
            {
                return true;
            }
            else
            {
                throw new Exception("Invalid assignment of value '" + query + "' to variable " + self.ReturnShallowValue() + " of type " + self.ReturnDeepVarType().Name); 
            }
        }
    }
    public abstract class TypeTemplate //Matching unit object
    {
        public TypeTemplate() { }
        private object _interiorContents;
        public abstract object contents //Abstract object - inherits type of the new typed item
        {
            get;
            set;
        }

        //Operation definitions

        public void SetItem(Item newItem)
        {
            contents = newItem.ReturnDeepValue(); 
        }
        public abstract Item AddOperation(Item self, Item ext);
        public abstract Item SubOperation(Item self, Item ext);
        public abstract Item MultOperation(Item self, Item ext);
        public abstract Item DivOperation(Item self, Item ext);

        //Comparitors
        public abstract Item LessThan(Item self, Item ext);
        public abstract Item GreaterThan(Item self, Item ext);
        public abstract Item EqualTo(Item self, Item ext);
        public Item NotEqualTo(Item self, Item ext)
        {
            return new Item(NodeContentType.Boolean, !Convert.ToBoolean(EqualTo(self, ext).ReturnDeepValue()));
        }

        //Bool Logic
        public abstract Item And(Item self, Item ext);
        public abstract Item Or(Item self, Item ext);
        public abstract Item Not(Item self);

        //Assignment conditions

        public abstract bool CanAssign(dynamic ext);

        public abstract bool IsValidValue(string ext);
        public sealed class Integer : TypeTemplate
        {
            public override object contents
            {
                get { return Convert.ToInt32(_interiorContents); }
                set { _interiorContents = value; }
            }
            public override Item AddOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Integer, self.ReturnDeepValue() + ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnDeepValue()) + ext.ReturnDeepValue());
                        }
                    case "String":
                        {
                            return new Item(NodeContentType.String, (self.ReturnDeepValue()).ToString() + ext.ReturnDeepValue().ToString());
                        }
                    default:
                        throw new Exception("Cannot add values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }

            public override Item SubOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Integer, self.ReturnDeepValue() - ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnDeepValue()) - ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot subtract values of type " + self.ReturnDeepVarType().Name  + " and " + ext.ReturnDeepVarType().Name );
                }
            }
            public override Item MultOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Integer, self.ReturnDeepValue() * ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnDeepValue()) * ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot multiply values of type " + self.ReturnDeepVarType().Name  + " and " + ext.ReturnDeepVarType().Name );
                }
            }
            public override Item DivOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnDeepValue()) / (float)(ext.ReturnDeepValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnDeepValue()) / ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot divide values of type " + self.ReturnDeepVarType().Name  + " and " + ext.ReturnDeepVarType().Name );
                }
            }

            public override Item LessThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() < ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, (float)self.ReturnDeepValue() < ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name  + " and " + ext.ReturnDeepVarType().Name );
                }
            }
            public override Item GreaterThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() > ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, (float)self.ReturnDeepValue() > ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name  + " and " + ext.ReturnDeepVarType().Name );
                }
            }

            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() == ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, (float)self.ReturnDeepValue() == ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name  + " and " + ext.ReturnDeepVarType().Name );
                }
            }
            public override Item And(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the and operation"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the or operation"); }
            public override Item Not(Item self) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the not operation"); }

            public override bool CanAssign(dynamic ext)
            {
                if(ext.GetType() == typeof(int)){ return true; }
                else { return false; }
            }
            public override bool IsValidValue(string ext)
            {
                int x = 0;
                if (int.TryParse(ext,out x)) { return true; }
                else { return false; }
            }
        }
        public sealed class Decimal : TypeTemplate
        {
            public override object contents
            {
                get { return (float)(Convert.ToDouble(_interiorContents)); }
                set { _interiorContents = value; }
            }

            public override Item AddOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() + (float)(ext.ReturnDeepValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() + ext.ReturnDeepValue());
                        }
                    case "String":
                        {
                            return new Item(NodeContentType.String, (self.ReturnDeepValue()).ToString() + ext.ReturnDeepValue().ToString());
                        }
                    default:
                        throw new Exception("Cannot add values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }

            public override Item SubOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() - (float)(ext.ReturnDeepValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() - ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot subtract values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }

            public override Item MultOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() * (float)(ext.ReturnDeepValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() * ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot multiply values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }

            public override Item DivOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() / (float)(ext.ReturnDeepValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnDeepValue() / ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot divide values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }

            public override Item LessThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() < (float)ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() < ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }
            public override Item GreaterThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() > (float)ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() > ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }

            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() == (float)ext.ReturnDeepValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() == ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }
            public override Item And(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the and operation"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the or operation"); ; }
            public override Item Not(Item self) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the not operation"); }
            public override bool CanAssign(dynamic ext)
            {
                if (ext.GetType() == typeof(float)) { return true; }
                else { return false; }
            }
            public override bool IsValidValue(string ext)
            {
                double x = 0;
                if (double.TryParse(ext, out x)) { return true; }
                else { return false; }
            }
        }
        public sealed class String : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents == null ? "" : _interiorContents.ToString(); }
                set { _interiorContents = value; }
            }

            public override Item AddOperation(Item self, Item ext)
            {
                return new Item(NodeContentType.String, self.ReturnDeepValue() + ext.ReturnDeepValue().ToString());
            }

            public override Item SubOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the subtract operation"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the multiply operation"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the divide operation"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the less than operation"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the greater than operation"); }
            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "String":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() == ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }
            public override Item And(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the and operation"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the or operation"); }
            public override Item Not(Item self) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the not operation"); }
            public override bool CanAssign(dynamic ext)
            {
                if (ext.GetType() == typeof(string)) { return true; }
                else { return false; }
            }

            public override bool IsValidValue(string ext)
            {
                return true; //May need reworking
            }
        }
        public sealed class Operation : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents == null ? "" : _interiorContents.ToString(); }
                set { _interiorContents = value; }
            }

            public override Item AddOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item SubOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item EqualTo(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item And(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Not(Item self) { throw new Exception("Unsupported interaction"); }
            public override bool CanAssign(dynamic ext)
            {
                throw new Exception("Assignment of operation item");
            }

            public override bool IsValidValue(string ext)
            {
                return false;
            }
        }
        public sealed class Boolean : TypeTemplate
        {
            public override object contents
            {
                get { return Convert.ToBoolean(_interiorContents); }
                set { _interiorContents = value; }
            }
            public override Item AddOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the add operation"); }
            public override Item SubOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the subtract operation"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the multiply operation"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the divide operation"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the less than operation"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Type " + self.ReturnDeepVarType().Name + " cannot use the greater than operation"); }
            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Boolean":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() == ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }
            public override Item And(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Boolean":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() && ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }
            public override Item Or(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Boolean":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnDeepValue() || ext.ReturnDeepValue());
                        }
                    default:
                        throw new Exception("Cannot compare values of type " + self.ReturnDeepVarType().Name + " and " + ext.ReturnDeepVarType().Name);
                }
            }
            public override Item Not(Item self)
            {
                return new Item(NodeContentType.Boolean, !Convert.ToBoolean(self.ReturnDeepValue()));
            }
            public override bool CanAssign(dynamic ext)
            {
                if (ext.GetType() == typeof(bool)) { return true; }
                else { return false; }
            }
            public override bool IsValidValue(string ext)
            {
                if (ext == "true" || ext == "false" || ext == "True" || ext == "False") { return true; }
                else { return false; }
            }
        }
        public sealed class End : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents == null ? "" : _interiorContents.ToString(); }
                set { _interiorContents = value; }
            }
            public override Item AddOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item SubOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item EqualTo(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item And(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Not(Item self) { throw new Exception("Unsupported interaction"); }
            public override bool CanAssign(dynamic ext)
            {
                throw new Exception("Assignment of end item");
            }
            public override bool IsValidValue(string ext)
            {
                return false;
            }
        }
        public sealed class Bracket : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents == null ? "" : _interiorContents.ToString(); }
                set { _interiorContents = value; }
            }
            public override Item AddOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item SubOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item EqualTo(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item And(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Not(Item self) { throw new Exception("Unsupported interaction"); }
            public override bool CanAssign(dynamic ext)
            {
                throw new Exception("Assignment of bracket item");
            }

            public override bool IsValidValue(string ext)
            {
                return false;
            }
        }
        public sealed class Identifier : TypeTemplate //Typeless identifier - unassigned.
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); } //Stores the variable name
                set { _interiorContents = value; }
            }
            public override Item AddOperation(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item SubOperation(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item EqualTo(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item And(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Invalid access of variable"); }
            public override Item Not(Item self) { throw new Exception("Invalid access of variable"); }
            public override bool CanAssign(dynamic ext)
            {
                throw new Exception("Assignment of shallow variable value");
            }
            public override bool IsValidValue(string ext)
            {
                return false;
            }
        }
    }
}