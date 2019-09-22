using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit To: http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute {
	//The name of the bool field that will be in control
	public string ConditionalSourceField = "";
	//TRUE = Hide in inspector / FALSE = Disable in inspector 
	public bool HideInInspector = false;
	//TRUE = Invert Conditional
	public bool InvertConditional = false;

	public ConditionalHideAttribute(string conditionalSourceField) {
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = false;
		this.InvertConditional = false;
	}

	public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector) {
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = hideInInspector;
		this.InvertConditional = false;
	}

	public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool invertConditional) {
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = hideInInspector;
		this.InvertConditional = invertConditional;
	}
}
