// -----------------------------------------------------------------------
//  <copyright file="IContentModerator.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for content moderator
    /// </summary>
    public interface IContentModerator
    {
        Task<IModeratorResult> Moderate(IModeratableContent content, IModeratorService service);
    }
}
