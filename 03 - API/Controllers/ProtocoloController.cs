using Consumer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProtocoloController : ControllerBase
{
    private readonly ProtocoloContext _context;

    public ProtocoloController(ProtocoloContext context)
    {
        _context = context;
    }

    [HttpGet("{numeroProtocolo}")]
    public async Task<ActionResult<Protocolo>> GetByNumeroProtocolo(string numeroProtocolo)
    {
        var protocolo = await _context.Protocolos.FirstOrDefaultAsync(p => p.NumeroProtocolo == numeroProtocolo);

        if (protocolo == null)
        {
            return NotFound();
        }

        return Ok(protocolo);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<IEnumerable<Protocolo>>> GetByCpf(string cpf)
    {
        var protocolos = await _context.Protocolos.Where(p => p.Cpf == cpf).ToListAsync();

        if (!protocolos.Any())
        {
            return NotFound();
        }

        return Ok(protocolos);
    }

    [HttpGet("rg/{rg}")]
    public async Task<ActionResult<IEnumerable<Protocolo>>> GetByRg(string rg)
    {
        var protocolos = await _context.Protocolos.Where(p => p.Rg == rg).ToListAsync();

        if (!protocolos.Any())
        {
            return NotFound();
        }

        return Ok(protocolos);
    }
}