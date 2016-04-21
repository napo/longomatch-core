//
//  Copyright (C) 2016 
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
using System.Xml.Serialization;

namespace LongoMatch.Addins.AddinsPathSerializer
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute ("xsd", "4.6.1055.0")]
	[System.SerializableAttribute ()]
	[System.Diagnostics.DebuggerStepThroughAttribute ()]
	[System.ComponentModel.DesignerCategoryAttribute ("code")]
	[System.Xml.Serialization.XmlTypeAttribute (AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute (Namespace = "", IsNullable = false)]
	public partial class Addins
	{

		private object[] itemsField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute ("Directory", typeof(AddinsDirectory), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		[System.Xml.Serialization.XmlElementAttribute ("Exclude", typeof(AddinsExclude), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public object[] Items {
			get {
				return this.itemsField;
			}
			set {
				this.itemsField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute ("xsd", "4.6.1055.0")]
	[System.SerializableAttribute ()]
	[System.Diagnostics.DebuggerStepThroughAttribute ()]
	[System.ComponentModel.DesignerCategoryAttribute ("code")]
	[System.Xml.Serialization.XmlTypeAttribute (AnonymousType = true)]
	public partial class AddinsDirectory
	{

		private string includesubdirsField;

		private string valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute ("include-subdirs")]
		public string includesubdirs {
			get {
				return this.includesubdirsField;
			}
			set {
				this.includesubdirsField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute ()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute ("xsd", "4.6.1055.0")]
	[System.SerializableAttribute ()]
	[System.Diagnostics.DebuggerStepThroughAttribute ()]
	[System.ComponentModel.DesignerCategoryAttribute ("code")]
	[System.Xml.Serialization.XmlTypeAttribute (AnonymousType = true)]
	public partial class AddinsExclude
	{

		private string valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute ()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}

