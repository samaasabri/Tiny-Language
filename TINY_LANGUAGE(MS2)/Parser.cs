using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_LANGUAGE_MS2_
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;
        List<Node> fun_nodes = new List<Node>();
        List<Node> state_nodes = new List<Node>();
        int check = 0, check2 = 0;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            // DataType();
            // program.Children.Add(Fun());
            //Function_Statment();
            if (fun_nodes.Count == 0)
            {
                Function_Statment();
            }
            if (fun_nodes.Count != 0)
            {
                foreach (Node i in fun_nodes)
                {

                    program.Children.Add(i);
                }
            }
            program.Children.Add(Main_Function());
            MessageBox.Show("Success");

            return program;
        }
        Node Fun()
        {
            Node fun = new Node("Fun");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type ==  Token_Type.T_main)
                {
                    fun.Children.Add(Main_Function());
                }
                else
                { foreach(Node i in Function_Statment())
                 {
                        fun.Children.Add(i);
                        // Fun();
                  }
                }
               

            }
            else
            {
                return null;
            }

            return fun;
        }
        // Implement your logic here
        List<Node> Function_Statment()
        {    
            Node function_Statment = new Node("function_Statment");
            //Function_declration();
            //Function_body();
            if (InputPointer < TokenStream.Count)
            {
                //function_Statment.Children.Add(Function_declration());
                //function_Statment.Children.Add(Function_body());
                if ((TokenStream[InputPointer].token_type == Token_Type.T_float || TokenStream[InputPointer].token_type == Token_Type.T_string|| TokenStream[InputPointer].token_type == Token_Type.T_int)&&(TokenStream[InputPointer+1].token_type!=Token_Type.T_main))
                {
                    function_Statment.Children.Add(Function_declration());
                    function_Statment.Children.Add(Function_body());
                    fun_nodes.Add(function_Statment);
                    Function_Statment();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return fun_nodes;
        }
        List<Node> State()
        {
            Node state = new Node("state");
            if (check == InputPointer)
            {
                match(Token_Type.T_end);
                return null;
            }
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer - 1].token_type == Token_Type.semeicolon) && (TokenStream[InputPointer].token_type != Token_Type.T_return &&TokenStream[InputPointer].token_type != Token_Type.T_end && TokenStream[InputPointer].token_type != Token_Type.T_until ))
                {

                    //state.Children.Add(match(Token_Type.semeicolon));
                    check = InputPointer;
                    state.Children.Add(Statement());
                    state_nodes.Add(state);
                    State();

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            return state_nodes;
        }
        List<Node> State(List<Node> n)
        {
            Node state = new Node("state");
            if (check2 == InputPointer)
            {
                Rdash();
                return null;
            }
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer - 1].token_type == Token_Type.semeicolon) && ( TokenStream[InputPointer].token_type != Token_Type.T_end && TokenStream[InputPointer].token_type != Token_Type.T_until && TokenStream[InputPointer].token_type != Token_Type.T_else && TokenStream[InputPointer].token_type != Token_Type.T_elseif))
                {

                    //state.Children.Add(match(Token_Type.semeicolon));
                    check2 = InputPointer;
                    state.Children.Add(Statement());
                    n.Add(state);
                    State(n);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            return state_nodes;
        }


        Node Function_body()
        {
            Node function_body = new Node("function_body");         
            function_body.Children.Add(match(Token_Type.LParanthesis));
            if(TokenStream[InputPointer].token_type != Token_Type.T_return)
            function_body.Children.Add(Statements());
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.T_return)
                {
                    function_body.Children.Add(Return_statement());
                     function_body.Children.Add(match(Token_Type.RParanthesis));
                }
                else
                {
                    Return_statement();
                    match(Token_Type.RParanthesis);
                }
            }
           
            //Statements();
            //Return_statement();

            return function_body;
        }
        Node Return_statement() {

            Node return_statement = new Node("return_statement");
            return_statement.Children.Add(match(Token_Type.T_return));
            return_statement.Children.Add(Expression());
           // Expression();
            return_statement.Children.Add(match(Token_Type.semeicolon));
            return return_statement;
        }
        Node Expression()
        {
            Node expression = new Node("expression");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.Strings)
                {
                    expression.Children.Add(match(Token_Type.Strings));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.number || TokenStream[InputPointer].token_type == Token_Type.LBracket || (TokenStream[InputPointer].token_type == Token_Type.Identifiers))
                {
                    expression.Children.Add(Equation());
                    // Equation();
                }
                //else if ((TokenStream[InputPointer].token_type == Token_Type.Identifiers))
                //{
                //    expression.Children.Add(Term());
                //   // Term();
                //}

            }
            else 
            {
                return null;
            }
          

            return expression;
        }
        Node Arithmtic_op()
        {
            Node arithmtic_op = new Node("arithmtic_op");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.PlusOp)
                {
                    arithmtic_op.Children.Add(match(Token_Type.PlusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.MinusOp )
                {
                    arithmtic_op.Children.Add(match(Token_Type.MinusOp));
                }
                else if ((TokenStream[InputPointer].token_type == Token_Type.DivideOp))
                {
                    arithmtic_op.Children.Add(match(Token_Type.DivideOp));
                }
                else if ((TokenStream[InputPointer].token_type == Token_Type.MultiplyOp))
                {
                    arithmtic_op.Children.Add(match(Token_Type.MultiplyOp));
                }

            }
            else
            {
                return null;
            }

            return arithmtic_op;
        }
        Node Term()
        {
            Node term = new Node("term");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.Identifiers)
                {
                    term.Children.Add(match(Token_Type.Identifiers));
                    term.Children.Add(Iden());
                }
                else if(TokenStream[InputPointer].token_type == Token_Type.number)
                {
                    term.Children.Add(match(Token_Type.number));
                }


            }
            else
            {
                return null;
            }

            //term.Children.Add(Function_call());
           // Function_call();
            return term;
        }
        Node Iden()
        {
            Node iden = new Node("iden'");
            //iden.Children.Add(Function_name());
            // Function_name();

            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.LBracket)
                {
                    iden.Children.Add(match(Token_Type.LBracket));
                    iden.Children.Add(Arguments());
                    iden.Children.Add(match(Token_Type.RBracket));
                }
                else
                {
                    return null;
                }


            }
            else
            {
                return null;
            }


            return iden;
        }
        //Node Function_call()
        //{
        //    Node function_call =new Node("Function_call");
        //    function_call.Children.Add(Function_name());
        //    // Function_name();
        //    function_call.Children.Add(match(Token_Type.LBracket));
        //    function_call.Children.Add(Arguments());
        //    //Arguments();
        //    function_call.Children.Add(match(Token_Type.RBracket));
        //    return function_call;
        //}
        Node Statements()
        {
            Node statements = new Node("statements");
            statements.Children.Add(Statement());
            // Statement();
            if (state_nodes.Count == 0)
            {
                State();
            }
            if (state_nodes.Count != 0)
            {
                foreach (Node i in state_nodes)
                {
                    statements.Children.Add(i);
                }
            }
           
            
          
             // State();
            return statements;
        }


        Node Statements(List<Node> n)
        {
            Node statements = new Node("statements");
            statements.Children.Add(Statement());
            // Statement();
            if (n.Count == 0)
            {
                State(n);
            }
            if (n.Count != 0)
            {
                foreach (Node i in n)
                {
                    statements.Children.Add(i);
                }
            }



            // State();
            return statements;
        }

        Node Statement()
        {
            Node statement = new Node("statement");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.T_int || TokenStream[InputPointer].token_type == Token_Type.T_float || TokenStream[InputPointer].token_type == Token_Type.T_string)
                {
                    statement.Children.Add(Declartion_Statment());
                    //Declartion_Statment();
                }
              
                else if ((TokenStream[InputPointer].token_type == Token_Type.T_read))
                {
                    statement.Children.Add(Read_Statment());
                    //Read_Statment();
                }
                else if ((TokenStream[InputPointer].token_type == Token_Type.T_write))
                {
                    statement.Children.Add(Write_Statment());
                    //Write_Statment();
                }

                else if (TokenStream[InputPointer].token_type == Token_Type.T_return)
                {
                    statement.Children.Add(Return_statement());
                    
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_if)
                {
                    statement.Children.Add(If_statement());
                    //If_statement();
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_repeat)
                {
                    statement.Children.Add(Repeat_statement());
                    //statement.Children.Add(match(Token_Type.T_until));
                    //Repeat_statement();
                }
                else if(TokenStream[InputPointer].token_type == Token_Type.Identifiers)
                {
                    statement.Children.Add(Assignment_Statment());
                    statement.Children.Add(match(Token_Type.semeicolon));
                    // Assignment_Statment();
                }
               
            }
            else
            {
                return null;
            }
            return statement;
        }
        Node Assignment_Statment()
        {
            Node asignment_Statment = new Node("asignment_Statment");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.Identifiers)
                {
                    asignment_Statment.Children.Add(match(Token_Type.Identifiers));
                    asignment_Statment.Children.Add(match(Token_Type.AssignmentOP));
                    asignment_Statment.Children.Add(Expression());
                }
               
            }
              
            
            
          // Expression();
            return asignment_Statment;
        }
        Node Read_Statment()
        {
            Node read_Statment = new Node("Read_Statment");
            read_Statment.Children.Add(match(Token_Type.T_read));
            read_Statment.Children.Add(match(Token_Type.Identifiers));
            read_Statment.Children.Add(match(Token_Type.semeicolon));
            return read_Statment;
        } 
        Node Write_Statment()
        {
            Node write_Statment = new Node("write_Statment");
            write_Statment.Children.Add(match(Token_Type.T_write));
            write_Statment.Children.Add(Edash());
            //Edash();
            write_Statment.Children.Add(match(Token_Type.semeicolon));
            return write_Statment;
        } 
        Node Edash()
        {
            Node edash = new Node("edash");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.Strings  || TokenStream[InputPointer].token_type == Token_Type.Identifiers || TokenStream[InputPointer].token_type == Token_Type.number || TokenStream[InputPointer].token_type == Token_Type.LBracket)
                {
                    edash.Children.Add(Expression());
                  //  Expression();
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_endl)
                {
                    edash.Children.Add(match(Token_Type.T_endl));
                }
                

            }
            else
            {
                return null;
            }

            return edash;
        }
        Node Declartion_Statment()
        {
            Node declartion_Statment = new Node("declartion_Statment");
            declartion_Statment.Children.Add(Data_type());
            //Data_type();
            declartion_Statment.Children.Add(Mdash());
            //Mdash();
            declartion_Statment.Children.Add(match(Token_Type.semeicolon));
            return declartion_Statment;
        } 
        Node Mdash()
        {
            Node mdash = new Node("mdash");
            //if (InputPointer < TokenStream.Count)
            //{
            //    if (TokenStream[InputPointer].token_type == Token_Type.Identifiers )
            //    {
            //        Identfier_list();
            //    }
            //    else if (TokenStream[InputPointer].token_type == Token_Type.T_endl)
            //    {
            //        
            //    }


            //}
            //else
            //{
            //    return null;
            //}
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.Identifiers && TokenStream[InputPointer+1].token_type == Token_Type.AssignmentOP)
                {
                    mdash.Children.Add(Assignment_statement_list());
                }
                else
                    mdash.Children.Add(Identfier_list());
            }
          
            
            //Identfier_list();
            //Assignment_statement_list();
            return mdash;
        }
        Node Repeat_statement()
        {
            Node repeat_statement = new Node("repeat_statement");
            repeat_statement.Children.Add(match(Token_Type.T_repeat));
            List<Node> repeat_statement_list = new List<Node>();
            repeat_statement.Children.Add(Statements(repeat_statement_list));
            //Statements();
            repeat_statement.Children.Add(match(Token_Type.T_until));
            repeat_statement.Children.Add(Condition_statement());
            //Condition_statement();
            return repeat_statement;
        }
        Node Condition_statement()
        {
            Node condition_statement = new Node("condition_statement");
            if (InputPointer < TokenStream.Count)
            {
                condition_statement.Children.Add(Condition());
                //Condition();
                condition_statement.Children.Add(Condition_statement_dash());
            }
            //Condition_statement_dash();
            return condition_statement;
        }
        Node Condition_statement_dash()
        {
            Node condition_statement_dash = new Node("condition_statement_dash");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.AndOp|| TokenStream[InputPointer].token_type == Token_Type.OrOp)
                {
                    condition_statement_dash.Children.Add(Boolean_op());
                    condition_statement_dash.Children.Add(Condition());
                    Condition_statement_dash();
                    //Boolean_op();
                    //Condition();
                    //Condition_statement_dash();
                }
                else
                {
                    return null;
                }


            }
            else
            {
                return null;
            }
           

            return condition_statement_dash;
        }
        Node Else_statement()
        {
            Node else_statement = new Node("else_statement");
            else_statement.Children.Add(match(Token_Type.T_else));
            List<Node> nodes_elseif = new List<Node>();
            else_statement.Children.Add(Statements(nodes_elseif));
          //  Statements();
            else_statement.Children.Add(match(Token_Type.T_end));
            return else_statement;
        }
        Node ElseIF_statement()
        {
            Node elseIF_statement = new Node("ElseIF_statement");
            if (InputPointer < TokenStream.Count)
            {
                elseIF_statement.Children.Add(match(Token_Type.T_elseif));
                elseIF_statement.Children.Add(Condition());
                //Condition();
                elseIF_statement.Children.Add(match(Token_Type.T_then));
                List<Node> nodes_elif = new List<Node>();
                elseIF_statement.Children.Add(Statements(nodes_elif));
                // Statements();
                elseIF_statement.Children.Add(Rdash());
                // Rdash();
            }
                return elseIF_statement;
          
        }
        Node Rdash()
        {
            Node rdash = new Node("rdash");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.T_else )
                {
                    rdash.Children.Add(Else_statement());
                   // Else_statement();
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_elseif)
                {
                    rdash.Children.Add(ElseIF_statement());
                    //ElseIF_statement();
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_end)
                {
                 
                    rdash.Children.Add(match(Token_Type.T_end));
                }
            }
            else
            {
                return null;
            }
         

            return rdash;
        }
        Node If_statement()
        {
            Node if_statement = new Node("if_statement");
            if (InputPointer < TokenStream.Count)
            {
                if_statement.Children.Add(match(Token_Type.T_if));
                if_statement.Children.Add(Condition_statement());
                //Condition();
                if_statement.Children.Add(match(Token_Type.T_then));
                List<Node> nodes_if = new List<Node>();
                if_statement.Children.Add(Statements(nodes_if));
                if_statement.Children.Add(Rdash());
            }
            //Statements();
            //Rdash();
            return if_statement;
        }
        Node Boolean_op()
        {
            Node boolean_op = new Node("boolean_op");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.OrOp)
                {
                    boolean_op.Children.Add(match(Token_Type.OrOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.AndOp)
                {
                    boolean_op.Children.Add(match(Token_Type.AndOp));
                }
                
            }
            else
            {
                return null;
            }

            
           
            return boolean_op;
        }
        Node Condition()
        {
            Node condition = new Node("condition");
            if (InputPointer < TokenStream.Count)
            {
                condition.Children.Add(match(Token_Type.Identifiers));
                condition.Children.Add(Condition_op());
                condition.Children.Add(Term());
            }
            //Condition_op();
            //Term();
            return condition;
        }
        Node Condition_op()
        {
            Node assignment_statement_list = new Node("assignment_statement_list");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.LessThanOp)
                {
                    assignment_statement_list.Children.Add(match(Token_Type.LessThanOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.GreaterThanOp)
                {
                    assignment_statement_list.Children.Add(match(Token_Type.GreaterThanOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.NotEqualOp)
                {
                    assignment_statement_list.Children.Add(match(Token_Type.NotEqualOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.EqualOp)
                {
                    assignment_statement_list.Children.Add(match(Token_Type.EqualOp));
                }

            }
            else
            {
                return null;
            }
            return assignment_statement_list;
        }
        Node Assignment_statement_list()
        {
            Node assignment_statement_list = new Node("assignment_statement_list");

            assignment_statement_list.Children.Add(Assignment_Statment());
            assignment_statement_list.Children.Add(Assi_state_lis());
       
            //Assignment_Statment();
            //Assi_state_lis();
            return assignment_statement_list;
        }

        Node Assi_state_lis()
        {
            Node assi_state_lis = new Node("assi_state_lis");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.comma)
                {
                    assi_state_lis.Children.Add(match(Token_Type.comma));
                    assi_state_lis.Children.Add(Assignment_Statment());
                    Assi_state_lis();
                    //Assignment_Statment();
                    //Assi_state_lis();
                }

            }
            else
            {
                return null;
            }
          
            return assi_state_lis;
        }
        Node Identfier_list()
        {
            Node identfier_list = new Node("identfier_list");
            identfier_list.Children.Add(match(Token_Type.Identifiers));
            identfier_list.Children.Add(Identfier_list_dash());
            //Identfier_list_dash();
            return identfier_list;
        }
        Node Identfier_list_dash()
        {
            Node identfier_list_dash = new Node("identfier_list_dash");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.comma)
                {
                    identfier_list_dash.Children.Add(match(Token_Type.comma));
                    identfier_list_dash.Children.Add(match(Token_Type.Identifiers));
                    Identfier_list_dash();
                }
                else
                {                   
                    return null;
                }

            }
            else
            {
                return null;
            }

            return identfier_list_dash;
        }

        Node DataType()
        {
            Node datatype = new Node("Datatype");
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer].token_type == Token_Type.T_int))
                {
                    datatype.Children.Add(match(Token_Type.T_int));
                   
                }
                else if((TokenStream[InputPointer].token_type == Token_Type.T_string))
                {
                    datatype.Children.Add(match(Token_Type.T_string));

                }
                else if ((TokenStream[InputPointer].token_type == Token_Type.T_float))
                {
                    datatype.Children.Add(match(Token_Type.T_float));

                }
                else
                {
                    return null;
                }
            }

            return datatype;
        }
       
       
        Node Function_declration()
        {
            Node function_declration = new Node("function_declration");
            function_declration.Children.Add(Data_type());
            function_declration.Children.Add(Function_name());
            function_declration.Children.Add(match(Token_Type.LBracket));
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type != Token_Type.RParanthesis) 
            {
                function_declration.Children.Add(Parameters());
            } 
            function_declration.Children.Add(match(Token_Type.RBracket));

            return function_declration;
        }
        Node Function_name()
        {
            Node function_name = new Node("function_name");
           
            function_name.Children.Add(match(Token_Type.Identifiers));
           
            return function_name;
        }
        Node Main_Function()
        {
            Node main_Function = new Node("main_function");
            if (InputPointer < TokenStream.Count)
            {
                main_Function.Children.Add(Data_type());
                main_Function.Children.Add(match(Token_Type.T_main));
                main_Function.Children.Add(match(Token_Type.LBracket));
                main_Function.Children.Add(match(Token_Type.RBracket));
                main_Function.Children.Add(Function_body());
            }
            return main_Function;
        }
        Node Parameters()
        { 
            Node parameters = new Node("parameters");
            parameters.Children.Add(DataType());
            parameters.Children.Add(match(Token_Type.Identifiers));
            parameters.Children.Add(Parames());
            return parameters;
        }
        Node Parames()
        {
            Node parames = new Node("parames");
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer].token_type == Token_Type.comma))
                {
                    parames.Children.Add(match(Token_Type.comma));
                    parames.Children.Add(DataType());
                    parames.Children.Add(match(Token_Type.Identifiers));
                    Parames();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            return parames;
        }
        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            arguments.Children.Add(match(Token_Type.Identifiers));
            arguments.Children.Add(Args());

            return arguments;
        }
        Node Args()
        {
            Node args = new Node("args");
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer].token_type == Token_Type.comma))
                {
                    args.Children.Add(match(Token_Type.comma));
                    args.Children.Add(match(Token_Type.Identifiers));
                    Args();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
           
            return args;
        }
        Node Equation()
        {
            Node equation = new Node("equation");
            
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer].token_type == Token_Type.Identifiers) || (TokenStream[InputPointer].token_type == Token_Type.number))
                {
                    equation.Children.Add(Term());
                    equation.Children.Add(Equa());

                }
                else if ((TokenStream[InputPointer].token_type == Token_Type.LBracket))
                {
                    equation.Children.Add(match(Token_Type.LBracket));
                    equation.Children.Add(Equation());
                    if ((TokenStream[InputPointer].token_type == Token_Type.RBracket))
                    {
                        equation.Children.Add(match(Token_Type.RBracket));
                        if (TokenStream[InputPointer].token_type == Token_Type.semeicolon)
                            return equation;
                        else if ((TokenStream[InputPointer].token_type == Token_Type.PlusOp) || (TokenStream[InputPointer].token_type == Token_Type.MinusOp) || (TokenStream[InputPointer].token_type == Token_Type.DivideOp) || (TokenStream[InputPointer].token_type == Token_Type.MultiplyOp))
                            equation.Children.Add(Equa());
                    }
                  
                }
               
            }
            else
            {
                return null;
            }
           
           
            return equation;
        }
        Node Equa()
        {
                Node equa = new Node("equa");
            if (InputPointer < TokenStream.Count)
            {
                if ((TokenStream[InputPointer].token_type == Token_Type.PlusOp) || (TokenStream[InputPointer].token_type == Token_Type.MinusOp) || (TokenStream[InputPointer].token_type == Token_Type.DivideOp) || (TokenStream[InputPointer].token_type == Token_Type.MultiplyOp))
                {
                    equa.Children.Add(Arithmtic_op());
                    equa.Children.Add(Equation());
                    Equa();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            return equa;
            
        }
        Node Data_type()
        {
            Node data_type = new Node("data_type");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Type.T_int)
                {
                    data_type.Children.Add(match(Token_Type.T_int));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_float)
                {
                    data_type.Children.Add(match(Token_Type.T_float));
                }
                else if (TokenStream[InputPointer].token_type == Token_Type.T_string)
                {
                    data_type.Children.Add(match(Token_Type.T_string));
                }
            }
            else
            {
                return null;
            }
            return data_type;
        }
        public Node match(Token_Type ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString()+ " "+TokenStream[InputPointer-1].lexeme);

                    return newNode;

                }

                else
                {
                    ERRORS.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                ERRORS.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
