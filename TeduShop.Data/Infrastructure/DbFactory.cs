namespace TeduShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private TeduShopDbContext dbContext;

        public TeduShopDbContext Init()
        {
            return dbContext ?? (dbContext = new TeduShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}