using System.IO;
/// <summary>
/// 本地语义函数
/// </summary>
public class LocalSemanticFunctionModel
{
    public LocalSemanticFunctionModel(
        string pathName, 
        string category, 
        string skillDir)
    {
        PathName = pathName;
        Category = category;
        SkillDir = skillDir;
        Name = new DirectoryInfo(Path.GetDirectoryName(pathName)).Name;
    }

    #region Properties
    /// <summary>
    /// Funcation Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// PathName
    /// </summary>
    public string PathName { get; }

    /// <summary>
    /// Funcation Description
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Skill Name
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Skill dir
    /// </summary>
    public string SkillDir { get; }

    public string RootDir => Directory.GetParent(SkillDir).FullName;
    #endregion

    public override string ToString() => Name;
}

