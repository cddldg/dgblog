using DG.Blog.Domain.Signature.Repositories;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DG.Blog.EntityFrameworkCore.Repositories.Signature
{
    /// <summary>
    /// SignatureRepository
    /// </summary>
    public class SignatureRepository : EfCoreRepository<DGBlogDbContext, Domain.Signature.Signature, Guid>, ISignatureRepository
    {
        public SignatureRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}