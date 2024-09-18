using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

public class ProtocoloContextFactory : IDesignTimeDbContextFactory<ProtocoloContext>
{
    public ProtocoloContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProtocoloContext>();
        //optionsBuilder.UseSqlServer("Server = RD2S\\MSSQLSERVER01; Database = ProtocoloDB; User Id = sa; Password = @dgd120M; Encrypt = False");
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ProtocoloDB;User Id=sa;Password=NovaSenhaForte123!;Encrypt=False");

        return new ProtocoloContext(optionsBuilder.Options);
    }
}