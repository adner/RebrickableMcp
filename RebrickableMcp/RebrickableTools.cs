using System.ComponentModel;
using ModelContextProtocol.Server;

namespace RebrickableMcp;

[McpServerToolType]
public static class RebrickableTools
{
    [McpServerTool, Description("Looks up a LEGO set on Rebrickable by its set number and returns the set name and number of pieces.")]
    public static async Task<object> GetLegoSet(
        RebrickableClient client,
        [Description("Rebrickable set number, typically suffixed (e.g. \"75192-1\"). If you only have the printed set number, append \"-1\".")] string setNumber,
        CancellationToken cancellationToken)
    {
        var set = await client.GetSetAsync(setNumber, cancellationToken);
        if (set is null)
        {
            return new
            {
                error = $"Set '{setNumber}' was not found. Rebrickable set numbers are usually suffixed with '-1' (e.g. '75192-1')."
            };
        }

        return new
        {
            setNumber = set.SetNum,
            name = set.Name,
            numParts = set.NumParts
        };
    }
}
