using System;
using Volo.Abp.Domain.Repositories;

namespace DG.Blog.Domain.Signature.Repositories
{
    /// <summary>
    /// ISignatureRepository
    /// </summary>
    public interface ISignatureRepository : IRepository<Signature, Guid>
    {
    }
}