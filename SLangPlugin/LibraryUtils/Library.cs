﻿using System;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;

// based on https://github.com/rsdn/nitra/

namespace SLangPlugin
{
    public class Library : IVsSimpleLibrary2
    {
        #region IVsSimpleLibrary2 Members

        public int AddBrowseContainer(VSCOMPONENTSELECTORDATA[] pcdComponent, ref uint pgrfOptions, out string pbstrComponentAdded)
        {
            pbstrComponentAdded = null;
            return VSConstants.E_NOTIMPL;
        }

        public int CreateNavInfo(SYMBOL_DESCRIPTION_NODE[] rgSymbolNodes, uint ulcNodes, out IVsNavInfo ppNavInfo)
        {
            ppNavInfo = null;
            return VSConstants.E_NOTIMPL;
        }

        public int GetBrowseContainersForHierarchy(IVsHierarchy pHierarchy, uint celt, VSBROWSECONTAINER[] rgBrowseContainers, uint[] pcActual)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int GetGuid(out Guid pguidLib)
        {
            pguidLib = MagicGuid;
            return VSConstants.S_OK;
        }

        public int GetLibFlags2(out uint pgrfFlags)
        {
            pgrfFlags = (uint)_LIB_FLAGS.LF_PROJECT | (uint)_LIB_FLAGS2.LF_SUPPORTSFILTERING | (uint)_LIB_FLAGS2.LF_SUPPORTSCALLBROWSER;
            //pgrfFlags = (uint)_LIB_FLAGS2.LF_SUPPORTSLISTREFERENCES;
            return VSConstants.S_OK;
        }

        #region Find All References

        /// <summary>
        /// Used to distinguish "Find All References" search from other search types.
        /// </summary>
        public const uint FindAllReferencesMagicNum = 0x11123334;

        public static readonly Guid MagicGuid = new Guid("1CC425ED-A93C-40B1-8A2B-672D9FE337DF");

        /// <summary>
        /// Stores existing "Find All References" results in the _findResults field.
        /// </summary>
        public void OnFindAllReferencesDone(IVsSimpleObjectList2 findResults)
        {
            _findResults = findResults;
        }

        private IVsSimpleObjectList2 _findResults = null;

        /// <summary>
        /// Handles Find All References, Calls From, Calls To ...
        /// </summary>
        public int GetList2(
          uint ListType,
          uint flags,
          VSOBSEARCHCRITERIA2[] pobSrch,
          out IVsSimpleObjectList2 ppIVsSimpleObjectList2)
        {
            if ((_findResults != null) && (pobSrch != null) && (pobSrch.Length == 1) && (pobSrch[0].dwCustom == FindAllReferencesMagicNum))
            {
                ppIVsSimpleObjectList2 = _findResults;
                _findResults = null;
                return VSConstants.S_OK;
            }

            ppIVsSimpleObjectList2 = null;
            return VSConstants.E_NOTIMPL;
        }

        #endregion

        public int GetSeparatorStringWithOwnership(out string pbstrSeparator)
        {
            pbstrSeparator = ".";
            return VSConstants.S_OK;
        }

        public int GetSupportedCategoryFields2(int category, out uint pCatField)
        {
            //pgrfCatField = (uint)_LIB_CATEGORY2.LC_HIERARCHYTYPE | (uint)_LIB_CATEGORY2.LC_PHYSICALCONTAINERTYPE;
            //return VSConstants.S_OK;
            pCatField = 0;

            switch (category)
            {
                case (int)LIB_CATEGORY.LC_MEMBERTYPE:
                    pCatField = (uint)_LIBCAT_MEMBERTYPE.LCMT_METHOD;
                    break;

                case (int)LIB_CATEGORY.LC_MEMBERACCESS:
                    pCatField = (uint)_LIBCAT_MEMBERACCESS.LCMA_PUBLIC |
                          (uint)_LIBCAT_MEMBERACCESS.LCMA_PRIVATE |
                          (uint)_LIBCAT_MEMBERACCESS.LCMA_PROTECTED |
                          (uint)_LIBCAT_MEMBERACCESS.LCMA_PACKAGE |
                          (uint)_LIBCAT_MEMBERACCESS.LCMA_FRIEND |
                          (uint)_LIBCAT_MEMBERACCESS.LCMA_SEALED;
                    break;

                case (int)LIB_CATEGORY.LC_LISTTYPE:
                    pCatField = (uint)_LIB_LISTTYPE.LLT_MEMBERS;
                    break;

                case (int)LIB_CATEGORY.LC_VISIBILITY:
                    pCatField = (uint)(_LIBCAT_VISIBILITY.LCV_VISIBLE |
                              _LIBCAT_VISIBILITY.LCV_HIDDEN);
                    break;

                default:
                    pCatField = (uint)0;
                    return Microsoft.VisualStudio.VSConstants.E_FAIL;
            }

            return VSConstants.S_OK;
        }

        public int LoadState(IStream pIStream, LIB_PERSISTTYPE lptType)
        {
            return VSConstants.S_OK;
        }

        public int RemoveBrowseContainer(uint dwReserved, string pszLibName)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int SaveState(IStream pIStream, LIB_PERSISTTYPE lptType)
        {
            return VSConstants.S_OK;
        }

        public int UpdateCounter(out uint pCurUpdate)
        {
            //return ((IVsSimpleObjectList2)_root).UpdateCounter(out pCurUpdate);
            pCurUpdate = 1;
            return VSConstants.E_NOTIMPL;
        }

        #endregion
    }
}

