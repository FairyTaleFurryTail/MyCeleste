using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAttribute : PropertyAttribute
{
   public string Title { get;private set; }
   public TitleAttribute(string str) { Title = str; }
}
