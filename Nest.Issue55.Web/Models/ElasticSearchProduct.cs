using System;

namespace Nest.Issue55.Web.Models
{
    [ElasticType( 
        Name = "product", 
        NumericDetection = true,
        SearchAnalyzer = "standard",
		IndexAnalyzer = "standard" )]
    public class ElasticSearchProduct
    {
        public Guid Id { get; set; }

        [ElasticProperty( OmitNorms = true, Index = FieldIndexOption.not_analyzed )]
        public string Format { get; set; }
    }
}