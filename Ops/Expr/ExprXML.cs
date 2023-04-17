using System.Xml.Linq;

namespace PSI;

public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral lit) {
      var litElement = new XElement ("Literal");
      litElement.SetAttributeValue ("Value", lit.Value.Text);
      litElement.SetAttributeValue ("Type", lit.Type);
      mXmlNode = litElement;
      return litElement;
   }

   public override XElement Visit (NIdentifier ident) {
      var idenElement = new XElement ("Ident");
      idenElement.SetAttributeValue ("Name", ident.Name.Text);
      idenElement.SetAttributeValue ("Type", ident.Type);
      mXmlNode = idenElement;
      return idenElement;
   }

   public override XElement Visit (NUnary unary) {
      var unaryElement = new XElement ("Unary");
      unaryElement.SetAttributeValue ("Op", unary.Op.Text);
      unaryElement.SetAttributeValue ("Type", unary.Type);
      var a = unary.Expr.Accept (this);
      unaryElement.Add (a);
      mXmlNode = unaryElement;
      return unaryElement;
   }

   public override XElement Visit (NBinary binary) {
      var binNode = new XElement ("Binary");
      binNode.SetAttributeValue ("Op", binary.Op.Kind.ToString ());
      binNode.SetAttributeValue ("Type", binary.Type);
      XElement a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      binNode.Add (a); binNode.Add (b);
      mXmlNode = binNode;
      return binNode;
   }

   public void SaveTo (string file) => File.WriteAllText (file, mXmlNode.ToString ());

   XElement mXmlNode = new ("MainNode");
}