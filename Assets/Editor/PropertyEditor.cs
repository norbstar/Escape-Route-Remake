#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Property))]
public class PropertyEditor : Editor
{
    private Property property;

    private void BooleanCondition(string label, Property.BooleanValue condition)
    {
        EditorGUILayout.BeginHorizontal("Box");

        var defaultBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = condition.include ? Color.grey : defaultBackgroundColor;
        condition.include = EditorGUILayout.Toggle(label, condition.include);
        GUI.backgroundColor = defaultBackgroundColor;
       
        if (condition.include)
        {
            GUI.backgroundColor = condition.shouldBeTrue ? Color.grey : defaultBackgroundColor;
            condition.shouldBeTrue = EditorGUILayout.Toggle("Qualifying State", condition.shouldBeTrue);
            GUI.backgroundColor = defaultBackgroundColor;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void MoveCondition(Property.MoveValue moveValue)
    {
        EditorGUILayout.BeginVertical("Box");

        moveValue.include = EditorGUILayout.BeginToggleGroup("Movement", moveValue.include);

        if (moveValue.include)
        {
            EditorGUI.indentLevel++;
            
            moveValue.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", moveValue.xAxis.include);

            if (moveValue.xAxis.include)
            {
                moveValue.xAxis.isZero = EditorGUILayout.Toggle("Is Zero", moveValue.xAxis.isZero);

                if (!moveValue.xAxis.isZero)
                {
                    moveValue.xAxis.nonZeroSign = (Property.SignEnum) EditorGUILayout.EnumFlagsField("Non Zero Sign", moveValue.xAxis.nonZeroSign);
                }
            }

            EditorGUILayout.EndToggleGroup();

            moveValue.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", moveValue.yAxis.include);

            if (moveValue.yAxis.include)
            {
                moveValue.yAxis.isZero = EditorGUILayout.Toggle("Is Zero", moveValue.yAxis.isZero);

                if (!moveValue.yAxis.isZero)
                {
                    moveValue.yAxis.nonZeroSign = (Property.SignEnum) EditorGUILayout.EnumFlagsField("Non Zero Sign", moveValue.yAxis.nonZeroSign);
                }
            }

            EditorGUILayout.EndToggleGroup();
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();
    }

    private void IsCrouching(Property property)
    {
        EditorGUILayout.BeginHorizontal("Box");

        var defaultBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = property.isCrouching.include ? Color.black : defaultBackgroundColor;
        property.isCrouching.include = EditorGUILayout.Toggle("Is Crouching", property.isCrouching.include);
        GUI.backgroundColor = defaultBackgroundColor;
       
        if (property.isCrouching.include)
        {
            GUI.backgroundColor = property.isCrouching.shouldBeTrue ? Color.black : defaultBackgroundColor;
            property.isCrouching.shouldBeTrue = EditorGUILayout.Toggle("Is True", property.isCrouching.shouldBeTrue);
            GUI.backgroundColor = defaultBackgroundColor;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void IsGrabbable(Property property)
    {
        var rect = EditorGUILayout.BeginHorizontal("Button");
        var style = new GUIStyle(GUI.skin.toggle);

        style.normal.textColor = property.isGrabbable.include ? Color.green : Color.grey;
        
        // if (GUI.Button(rect, GUIContent.none))
        // {
        //     property.isGrabbable.include = !property.isGrabbable.include;
            // style.normal.textColor = Color.black;
        // }
        // else
        // {
        //     style.normal.textColor = Color.green;
        // }

        // EditorGUILayout.LabelField("Is Grabbable", style);
        property.isGrabbable.include = EditorGUILayout.Toggle("Is Grabbable", property.isGrabbable.include, style);
        // style.normal.textColor = Color.black;

        if (property.isGrabbable.include)
        {
            property.isGrabbable.shouldBeTrue = EditorGUILayout.Toggle("Required True", property.isGrabbable.shouldBeTrue);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddButton()
    {
        var rect = EditorGUILayout.BeginHorizontal("Button");

        // EditorGUILayout.LabelField("Click Me", EditorStyles.boldLabel);

        if (GUI.Button(rect, GUIContent.none))
        {
            Debug.Log("Clicked!");
        }

        GUILayout.Label("Click Me");

        EditorGUILayout.EndHorizontal();
    }

    private void AddAltButton()
    {
#if false
        var style = new GUIStyle(GUI.skin.button);
        style.alignment = TextAnchor.MiddleRight;
        style.fixedWidth = 70;

        // var rect = EditorGUILayout.BeginHorizontal("Button");
        var rect = EditorGUILayout.BeginHorizontal(style);

        if (GUI.Button(rect, GUIContent.none))
        {
            Debug.Log("Clicked!");
        }

        style = new GUIStyle(GUI.skin.label);
        style.fontStyle = FontStyle.Bold;

        // EditorGUILayout.LabelField("Click Me", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Click Me", style);

        EditorGUILayout.EndHorizontal();
#endif
        // GUILayout.Button("Click Me", EditorStyles.miniButtonLeft, GUILayout.Width(85));

        var rect = EditorGUILayout.BeginHorizontal();
        
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Click Me", EditorStyles.miniButtonLeft, GUILayout.Width(85)))
        {
            Debug.Log("Clicked!");
        }

        // if (GUI.Button(rect, GUIContent.none))
        // {
        //     Debug.Log("Clicked!");
        // }

        EditorGUILayout.EndHorizontal();
    }

    private void AddBooleans()
    {
        EditorGUILayout.LabelField("Booleans", EditorStyles.boldLabel);
        BooleanCondition("Is Crouching", property.isCrouching);
        BooleanCondition("Is Grabbable", property.isGrabbable);
        BooleanCondition("Is Traversable", property.isTraversable);
        BooleanCondition("Is Holding", property.isHolding);
        BooleanCondition("Is Dashing", property.isDashing);
        BooleanCondition("Is Sliding", property.isSliding);
        BooleanCondition("Is Input Suspended", property.isInputSuspended);
        BooleanCondition("Is Blocked Top", property.isBlockedTop);
        BooleanCondition("Is Blocked Right", property.isBlockedRight);
        BooleanCondition("Is Grounded", property.isGrounded);
        BooleanCondition("Is Blocked Left", property.isBlockedLeft);
    }

    private void AddList()
    {
        property.Foo = (Property.FooEnum) EditorGUILayout.EnumPopup("Foo", property.Foo);
        
        var rect = EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        // if (GUILayout.Button(property.BtnTexture, EditorStyles.miniButtonLeft, GUILayout.Width(85)))
        if (GUILayout.Button("Click Me", EditorStyles.miniButtonLeft, GUILayout.Width(85)))
        {
            Debug.Log($"Clicked {property.Foo}!");
        }

        EditorGUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        property = (Property) target;

        // var rect = EditorGUILayout.BeginHorizontal("Box");
        // var style = new GUIStyle(EditorStyles.boldLabel);
        // style.alignment = TextAnchor.MiddleLeft;
        // style.normal.textColor = property.isCrouching.include ? Color.blue : Color.grey;
        // EditorGUILayout.LabelField("Is Crouching", style);
        // property.isCrouching.include = EditorGUILayout.Toggle(property.isCrouching.include);
        // EditorGUILayout.EndHorizontal();
#if false
        EditorGUILayout.LabelField("Booleans", EditorStyles.boldLabel);
        BooleanCondition("Is Crouching", property.isCrouching);
        BooleanCondition("Is Grabbable", property.isGrabbable);
        BooleanCondition("Is Traversable", property.isTraversable);
        BooleanCondition("Is Holding", property.isHolding);
        BooleanCondition("Is Dashing", property.isDashing);
        BooleanCondition("Is Sliding", property.isSliding);
        BooleanCondition("Is Input Suspended", property.isInputSuspended);
        BooleanCondition("Is Blocked Top", property.isBlockedTop);
        BooleanCondition("Is Blocked Right", property.isBlockedRight);
        BooleanCondition("Is Grounded", property.isGrounded);
        BooleanCondition("Is Blocked Left", property.isBlockedLeft);

        MoveCondition(property.moveValue);
#endif
        AddBooleans();
        EditorGUILayout.Space();

        // AddButton();
        // AddAltButton();

        AddList();

        // IsCrouching(property);
        // IsGrabbable(property);
#if false
        Rect rect;

        rect = EditorGUILayout.BeginHorizontal("Button");
        // if (GUI.Button(rect, GUIContent.none))
        // {
        //     Debug.Log("Go here");
        // }

        // GUILayout.Label("I'm inside the button");
        // GUILayout.Label("So am I");
        property.isCrouching.include = EditorGUILayout.Toggle("Is Crouching", property.isCrouching.include);

        if (property.isCrouching.include)
        {
            property.isCrouching.@true = EditorGUILayout.Toggle("True", property.isCrouching.@true);
        }
        EditorGUILayout.EndHorizontal();
#endif

#if false
        var rect = EditorGUILayout.BeginHorizontal("Button");
        // GUILayout.Label("So am I");
        // EditorGUILayout.LabelField("Foo", EditorStyles.boldLabel);
        // EditorGUILayout.LabelField("Bar", EditorStyles.boldLabel);
        property.isCrouching.include = EditorGUILayout.Toggle("Is Crouching", property.isCrouching.include);

        if (property.isCrouching.include)
        {
            property.isCrouching.@true = EditorGUILayout.Toggle("True", property.isCrouching.@true);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.LabelField("Booleans", EditorStyles.boldLabel);

        property.isCrouching.include = EditorGUILayout.BeginToggleGroup("Is Crouching", property.isCrouching.include);

        if (property.isCrouching.include)
        {
            property.isCrouching.@true = EditorGUILayout.Toggle("True", property.isCrouching.@true);
        }

        EditorGUILayout.EndToggleGroup();
#endif
        // property.isCrouching = EditorGUILayout.Toggle("Is Crouching", property.isCrouching);
        // property.isGrabbable = EditorGUILayout.Toggle("Is Grabbable", property.isGrabbable);
        // property.isTraversable = EditorGUILayout.Toggle("Is Traversable", property.isTraversable);
        // property.isHolding = EditorGUILayout.Toggle("Is Holding", property.isHolding);
        // property.isDashing = EditorGUILayout.Toggle("Is Dashing", property.isDashing);
        // property.isSliding = EditorGUILayout.Toggle("Is Sliding", property.isSliding);
        // property.isInputSuspended = EditorGUILayout.Toggle("Is InputSuspended", property.isInputSuspended);
        // property.isBlockedTop = EditorGUILayout.Toggle("Is BlockedTop", property.isBlockedTop);
        // property.isBlockedRight = EditorGUILayout.Toggle("Is BlockedRight", property.isBlockedRight);
        // property.isGrounded = EditorGUILayout.Toggle("Is Grounded", property.isGrounded);
        // property.isBlockedLeft = EditorGUILayout.Toggle("Is BlockedLeft", property.isBlockedLeft);
#if false
        EditorGUILayout.Space();

        // EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
        property.moveValue.include = EditorGUILayout.BeginToggleGroup("Movement", property.moveValue.include);

        if (property.moveValue.include)
        {
            EditorGUI.indentLevel++;
            
            property.moveValue.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", property.moveValue.xAxis.include);

            if (property.moveValue.xAxis.include)
            {
                property.moveValue.xAxis.isZero = EditorGUILayout.Toggle("Is Zero", property.moveValue.xAxis.isZero);
                property.moveValue.xAxis.nonZeroSign = (Property.SignEnum) EditorGUILayout.EnumFlagsField("Non Zero Sign", property.moveValue.xAxis.nonZeroSign);
            }

            EditorGUILayout.EndToggleGroup();

            property.moveValue.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", property.moveValue.yAxis.include);

            if (property.moveValue.yAxis.include)
            {
                property.moveValue.yAxis.isZero = EditorGUILayout.Toggle("Is Zero", property.moveValue.yAxis.isZero);
                property.moveValue.yAxis.nonZeroSign = (Property.SignEnum) EditorGUILayout.EnumFlagsField("Non Zero Sign", property.moveValue.yAxis.nonZeroSign);
            }

            EditorGUILayout.EndToggleGroup();
        }

        EditorGUILayout.EndToggleGroup();
#endif

        // base.OnInspectorGUI();
    }
}
#endif
