using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortalElixirHand.Model
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string PictureUrl { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
