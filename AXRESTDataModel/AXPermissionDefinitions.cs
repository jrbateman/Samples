using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTDataModel
{
    /// <summary>
    /// A list of all AX permissions
    /// </summary>
    public class AXPermissionDescriptions : KeyValueCollection
    {
    }

    /// <summary>
    /// Represents an AX Permissions collection
    /// </summary>
    public class AXPermissionCollection : KeyBooleanCollection
    {
    }

    public enum AXPermissions
    {
        ScanOnLine,
        BatchScan,
        BatchIndex,
        ModifyIndex,
        AddObject,
        AddPage,
        DeleteDoc,
        DeletePage,
        DeleteObject,
        CreateApp,
        ModifyApp,
        DeleteApp,
        UserSecurityAdmin,
        DocLevelSecAdmin,
        AutoIndexFileMaint,
        KeyRefFileMaint,
        COLDImportMaint,
        COLDImport,
        View,
        FaxIn,
        FaxOut,
        Print,
        AutoIndexImport,
        RefFileImport,
        MigrateApp,
        ImageUtilities,
        IndexImageImport,
        AEAdmin,
        Config,
        EditRedactions,
        EditAnnotations,
        MultiLogins,
        FullTextIndex,
        FullTextQuery,
        OCR,
        ScanFix,
        BatchExtract,
        GlobalAnnot,
        WxPALicense,
        CXReports,
        CreateAnnotations,
        CreateRedactions,
        RetentionUser,
        RetentionAdmin,
    }
}
