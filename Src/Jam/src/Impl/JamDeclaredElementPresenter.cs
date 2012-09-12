using System;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  [PsiSharedComponent]
  internal class JamDeclaredElementPresenter : IDeclaredElementPresenter
  {
    public static JamDeclaredElementPresenter Instance { get { return PsiShared.GetComponent<JamDeclaredElementPresenter>(); } }

    public string Format(DeclaredElementPresenterStyle style, IDeclaredElement declaredElement, ISubstitution substitution, out DeclaredElementPresenterMarking marking)
    {
      if (!declaredElement.IsValid())
        throw new ArgumentException("declaredElement should be valid", "declaredElement");

      var cssDeclaredElement = declaredElement as IJamDeclaredElement;
      if (cssDeclaredElement == null)
        throw new ArgumentException("declaredElement should have jam language", "declaredElement");

      var result = new StringBuilder();
      marking = new DeclaredElementPresenterMarking();

      if (style.ShowEntityKind != EntityKindForm.NONE)
      {
        var entityKind = cssDeclaredElement.GetElementType().PresentableName;
        if (!entityKind.IsNullOrEmpty())
        {
          if (style.ShowEntityKind == EntityKindForm.NORMAL_IN_BRACKETS)
            entityKind = "(" + entityKind + ")";

          marking.EntityKindRange = AppendString(result, entityKind);
          result.Append(" ");
        }
      }

      if (style.ShowNameInQuotes)
        result.Append("\'");

      if (style.ShowName != NameStyle.NONE)
      {
        var elementName = cssDeclaredElement.ShortName;

        if (elementName == SharedImplUtil.MISSING_DECLARATION_NAME)
          elementName = "<unknown name>";

        marking.NameRange = AppendString(result, elementName);
      }

      if (style.ShowParameterNames || style.ShowParameterTypes)
      {
        DeclaredElementPresenterMarking.Parameter[] ranges;

        string paramList = GetParametersListStr(style, declaredElement, substitution, out ranges);
        if (paramList != string.Empty)
        {
          TrimString(result);
          int nParams = ranges.Length;
          int offset = result.Length;
          marking.ParameterRanges = new DeclaredElementPresenterMarking.Parameter[nParams];
          for (int i = 0; i < nParams; i++)
            marking.ParameterRanges[i] = new DeclaredElementPresenterMarking.Parameter(ranges[i], offset);

          marking.ParameterListRange = AppendString(result, paramList);
          result.Append(" ");
        }
      }

      if (style.ShowNameInQuotes)
      {
        if (result[result.Length - 1] == '\'')
          result.Remove(result.Length - 1, 1);
        else
        {
          TrimString(result);
          result.Append("\' ");
        }
      }

      if (style.ShowParameterContainer != ParameterContainerStyle.NONE)
      {
        TextRange containerNameRange;
        var containerStr = GetContainerName(declaredElement, style, substitution, out containerNameRange);
        TrimString (result);
        result.Append (" ");
        marking.ContainerRange = AppendString (result, containerStr);
        marking.ContainerNameRange = new TextRange(marking.ContainerRange.StartOffset + containerNameRange.StartOffset, 
                                                    marking.ContainerRange.StartOffset + containerNameRange.EndOffset);
      }

      TrimString(result);
      return result.ToString();
    }

    private string GetContainerName(IDeclaredElement declaredElement, DeclaredElementPresenterStyle presenter, ISubstitution substitution, out TextRange containerNameRange)
    {
      containerNameRange = TextRange.InvalidRange;

      var parameter = declaredElement as IParameterDeclaredElement;
      if (parameter != null)
      {
        if (presenter.ShowParameterContainer == ParameterContainerStyle.NONE)
          return String.Empty;

        var procedure = parameter.ContainingProcedure;
        if (procedure != null) 
        {
          DeclaredElementPresenterMarking marking;
          var containerName = Format(presenter, procedure, substitution, out marking);
          if (containerName.IsNullOrEmpty())
            return String.Empty;

          var container = String.Empty;
          switch (presenter.ShowParameterContainer)
          {
            case ParameterContainerStyle.NONE:
              return String.Empty;
            case ParameterContainerStyle.AFTER:
              container = "of ";
              break;
            case ParameterContainerStyle.AFTER_IN_PARENTHESIS:
              container = "(of ";
              break;
          }

          containerNameRange = new TextRange(container.Length, container.Length + containerName.Length);
          container += containerName;

          if (presenter.ShowParameterContainer == ParameterContainerStyle.AFTER_IN_PARENTHESIS)
            container += ")";

          return container;
        }
      }

      return string.Empty;
    }

    private static string GetParametersListStr (DeclaredElementPresenterStyle presenter, IDeclaredElement element, ISubstitution substitution, out DeclaredElementPresenterMarking.Parameter[] ranges) 
    {
      if (!(element is IProcedureDeclaredElement))
      {
        ranges = null;
        return string.Empty;
      }

      var procedureDeclaredElement = (IProcedureDeclaredElement)element;

      var str = new StringBuilder();
      str.Append('(');

      var parameters = procedureDeclaredElement.Parameters;

      ranges = new DeclaredElementPresenterMarking.Parameter[parameters.Count];
      for (int i = 0; i < parameters.Count; i++)
      {
        ranges[i] = new DeclaredElementPresenterMarking.Parameter();
        FormatParameter(presenter, parameters[i], str, ranges[i]);
        if (i < parameters.Count - 1)
          str.Append(", ");
      }

      str.Append(")");
      return str.ToString();
    }

    private static void FormatParameter(DeclaredElementPresenterStyle presenter, IParameterDeclaredElement param, StringBuilder str, DeclaredElementPresenterMarking.Parameter range)
    {
      range.KindRange = TextRange.InvalidRange;
      range.TypeRange = TextRange.InvalidRange;
      range.DefaultValueRange = TextRange.InvalidRange;
      range.ScalarTypeRange = TextRange.InvalidRange;

      var paramName = presenter.ShowParameterNames ? param.ShortName : string.Empty;
      range.NameRange = AppendString(str, paramName);
    }

    public string Format(ParameterKind parameterKind)
    {
      return String.Empty;
    }

    public string Format(AccessRights accessRights)
    {
      return String.Empty;
    }

    private static TextRange AppendString(StringBuilder sb, string substr)
    {
      int s = sb.Length;
      sb.Append(substr);
      return substr.Length == 0 ? TextRange.InvalidRange : new TextRange(s, sb.Length);
    }

    private static void TrimString(StringBuilder str)
    {
      while (str.Length > 0 && str[str.Length - 1] == ' ')
        str.Remove(str.Length - 1, 1);
    }
  }
}