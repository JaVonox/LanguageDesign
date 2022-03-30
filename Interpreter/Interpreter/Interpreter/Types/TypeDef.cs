using System;
using System.Collections.Generic;
using System.Text;
using Nodes;
using Tokens;

namespace TypeDef
{
    public class Item
    {
        private TypeTemplate content { get; set; }

        public dynamic ReturnValue() //Return the type-adjusted contents of the item
        {
            return content.contents;
        }

        public Type GetType() //Return the value type
        {
            return content.GetType();
        }

        public string GetStringContents()
        {
            return ReturnValue().ToString();
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
                case NodeContentType.Identifier: //Runs when variable is not in global area. When assigned to 
                    {
                        content = new TypeTemplate.Identifier();
                        content.contents = inContents; //contents is variable name
                        break;
                    }
                default:
                    throw new Exception("Unimplemented type: " + type + ":" + inContents);
            }
        }

        private static void IsInvalid(Item self, Item ext)
        {
            if(self.GetType() == typeof(TypeTemplate.Identifier) || ext.GetType() == typeof(TypeTemplate.Identifier)) { throw new Exception("Use of variable before assignment"); }
        }
        //Equations

        public static Item operator +(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.AddOperation(self, ext);
        }
        public static Item operator -(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.SubOperation(self, ext);
        }
        public static Item operator *(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.MultOperation(self, ext);
        }
        public static Item operator /(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.DivOperation(self, ext);
        }

        //Value checking
        public static Item LessThan(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.LessThan(self, ext);
        }
        public static Item GreaterThan(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.GreaterThan(self, ext);
        }
        public static Item EqualTo(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.EqualTo(self, ext);
        }
        public static Item NotEqualTo(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.NotEqualTo(self, ext);
        }
        public static Item LessThanEqualTo(Item self, Item ext)
        {
            IsInvalid(self, ext);
            Item lessOp = self.content.LessThan(self, ext);
            if (lessOp.ReturnValue()) { return lessOp; }
            Item equalOp = self.content.EqualTo(self, ext);
            return equalOp;
        }
        public static Item GreaterThanEqualTo(Item self, Item ext)
        {
            IsInvalid(self, ext);
            Item gOp = self.content.GreaterThan(self, ext);
            if (gOp.ReturnValue()) { return gOp; }
            Item equalOp = self.content.EqualTo(self, ext);
            return equalOp;
        }

        //Boolean
        public static Item And(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.And(self, ext);
        }
        public static Item Or(Item self, Item ext)
        {
            IsInvalid(self, ext);
            return self.content.Or(self, ext);
        }
        public static Item Not(Item self)
        {
            if (Nodes.Node.contentRef[self.GetType()] == NodeContentType.Identifier) { throw new Exception("Use of variable before assignment"); }
            return self.content.Not(self);
        }
        public static void SetContent(Item self, Item newContents)
        {
            self.content.SetItem(newContents);
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
            contents = newItem.ReturnValue(); //Set the value. TODO add type checking
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
            return new Item(NodeContentType.Boolean, !Convert.ToBoolean(EqualTo(self, ext).ReturnValue()));
        }

        //Bool Logic
        public abstract Item And(Item self, Item ext);
        public abstract Item Or(Item self, Item ext);
        public abstract Item Not(Item self);
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
                            return new Item(NodeContentType.Integer, self.ReturnValue() + ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnValue()) + ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item SubOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Integer, self.ReturnValue() - ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnValue()) - ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item MultOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Integer, self.ReturnValue() * ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnValue()) * ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item DivOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnValue()) / (float)(ext.ReturnValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, (float)(self.ReturnValue()) / ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item LessThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() < ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, (float)self.ReturnValue() < ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item GreaterThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() > ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, (float)self.ReturnValue() > ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() == ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, (float)self.ReturnValue() == ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item And(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Not(Item self) { throw new Exception("Unsupported interaction"); }

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
                            return new Item(NodeContentType.Decimal, self.ReturnValue() + (float)(ext.ReturnValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() + ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item SubOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() - (float)(ext.ReturnValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() - ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item MultOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() * (float)(ext.ReturnValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() * ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item DivOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() / (float)(ext.ReturnValue()));
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Decimal, self.ReturnValue() / ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item LessThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() < (float)ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() < ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item GreaterThan(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() > (float)ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() > ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Integer":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() == (float)ext.ReturnValue());
                        }
                    case "Decimal":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() == ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item And(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Not(Item self) { throw new Exception("Unsupported interaction"); }
        }
        public sealed class String : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); }
                set { _interiorContents = value; }
            }

            public override Item AddOperation(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "String":
                        {
                            return new Item(NodeContentType.String, self.ReturnValue() + ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }

            public override Item SubOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "String":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() == ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item And(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Or(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item Not(Item self) { throw new Exception("Unsupported interaction"); }
        }
        public sealed class Operation : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); }
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
        }
        public sealed class Boolean : TypeTemplate
        {
            public override object contents
            {
                get { return Convert.ToBoolean(_interiorContents); }
                set { _interiorContents = value; }
            }
            public override Item AddOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item SubOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item MultOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item DivOperation(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item LessThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item GreaterThan(Item self, Item ext) { throw new Exception("Unsupported interaction"); }
            public override Item EqualTo(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Boolean":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() == ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item And(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Boolean":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() && ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item Or(Item self, Item ext)
            {
                switch (ext.GetType().Name)
                {
                    case "Boolean":
                        {
                            return new Item(NodeContentType.Boolean, self.ReturnValue() || ext.ReturnValue());
                        }
                    default:
                        throw new Exception("Unsupported interaction");
                }
            }
            public override Item Not(Item self)
            {
                return new Item(NodeContentType.Boolean, !Convert.ToBoolean(self.ReturnValue()));
            }

        }
        public sealed class End : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); }
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
        }
        public sealed class Bracket : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); }
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
        }
        public sealed class Identifier : TypeTemplate //Typeless identifier - unassigned.
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); } //Stores the variable name
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
        }
    }
}