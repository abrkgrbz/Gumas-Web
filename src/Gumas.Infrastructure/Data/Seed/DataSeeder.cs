using Gumas.Domain.Entities;
using Gumas.Domain.Enums;

namespace Gumas.Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(GumasDbContext context)
    {
        if (!context.Brands.Any())
        {
            await SeedBrandsAsync(context);
        }
        else
        {
            await EnsureBrandsAsync(context);
        }

        if (!context.Categories.Any())
        {
            await SeedCategoriesAsync(context);
        }

        if (!context.Products.Any())
        {
            await SeedProductsAsync(context);
        }

        if (!context.Sliders.Any())
        {
            await SeedSlidersAsync(context);
        }

        if (!context.Settings.Any())
        {
            await SeedSettingsAsync(context);
        }

        await EnsureLogoSettingsAsync(context);

        if (!context.News.Any())
        {
            await SeedNewsAsync(context);
        }

        if (!context.TeamMembers.Any())
        {
            await SeedTeamMembersAsync(context);
        }

        if (!context.Certificates.Any())
        {
            await SeedCertificatesAsync(context);
        }
    }

    private static async Task SeedNewsAsync(GumasDbContext context)
    {
        var news = new List<News>
        {
            new()
            {
                NameTr = "JOST 2024 Yeni Ürün Serisini Tanıttı",
                NameEn = "JOST Introduces 2024 New Product Series",
                Slug = "jost-2024-yeni-urun-serisi",
                SummaryTr = "JOST, 2024 yılı için geliştirdiği yeni beşinci teker ve çeki sistemleri serisini Automechanika fuarında tanıttı.",
                SummaryEn = "JOST introduced its new fifth wheel and coupling systems series developed for 2024 at the Automechanika fair.",
                ContentTr = "<p>JOST, ağır vasıta sektörünün en önemli fuarlarından Automechanika'da 2024 model yılı için geliştirdiği yeni ürün serisini tanıttı.</p><p>Yeni JSK 42 serisi beşinci tekerlekler, artırılmış yük kapasitesi ve geliştirilmiş kilitleme mekanizması ile dikkat çekiyor. Ayrıca yeni nesil sensör entegrasyonu sayesinde araç takip sistemleriyle uyumlu çalışabiliyor.</p>",
                ContentEn = "<p>JOST introduced its new product series developed for the 2024 model year at Automechanika, one of the most important fairs in the heavy vehicle industry.</p><p>The new JSK 42 series fifth wheels stand out with increased load capacity and improved locking mechanism. Additionally, thanks to the new generation sensor integration, they can work compatible with vehicle tracking systems.</p>",
                ImageUrl = "/images/news/jost-2024.jpg",
                ThumbnailUrl = "/images/news/jost-2024-thumb.jpg",
                PublishDate = DateTime.Now.AddDays(-5),
                IsFeatured = true,
                ViewCount = 245
            },
            new()
            {
                NameTr = "Gümaş Otomotiv ISO 9001:2015 Sertifikasını Yeniledi",
                NameEn = "Gümaş Automotive Renewed ISO 9001:2015 Certificate",
                Slug = "gumas-iso-9001-sertifika-yenileme",
                SummaryTr = "Kalite yönetim sistemimiz başarıyla denetlendi ve ISO 9001:2015 sertifikamız yenilendi.",
                SummaryEn = "Our quality management system was successfully audited and our ISO 9001:2015 certificate was renewed.",
                ContentTr = "<p>Gümaş Otomotiv olarak kaliteye verdiğimiz önemin bir göstergesi olarak ISO 9001:2015 sertifikamızı başarıyla yeniledik.</p><p>Bağımsız denetim kuruluşu tarafından gerçekleştirilen kapsamlı denetim sonucunda, kalite yönetim sistemimizin uluslararası standartlara tam uygunluğu tescil edildi.</p>",
                ContentEn = "<p>As Gümaş Automotive, we have successfully renewed our ISO 9001:2015 certificate as an indicator of our commitment to quality.</p><p>As a result of the comprehensive audit conducted by the independent audit organization, the full compliance of our quality management system with international standards was certified.</p>",
                ImageUrl = "/images/news/iso-certificate.jpg",
                ThumbnailUrl = "/images/news/iso-certificate-thumb.jpg",
                PublishDate = DateTime.Now.AddDays(-12),
                IsFeatured = false,
                ViewCount = 128
            },
            new()
            {
                NameTr = "TRIDEC Direksiyon Sistemleri Eğitimi Gerçekleştirildi",
                NameEn = "TRIDEC Steering Systems Training Completed",
                Slug = "tridec-direksiyon-sistemleri-egitimi",
                SummaryTr = "Teknik ekibimiz TRIDEC fabrikasında ileri düzey direksiyon sistemleri eğitimi aldı.",
                SummaryEn = "Our technical team received advanced steering systems training at the TRIDEC factory.",
                ContentTr = "<p>Gümaş Otomotiv teknik ekibi, Hollanda'daki TRIDEC fabrikasında düzenlenen ileri düzey direksiyon sistemleri eğitimine katıldı.</p><p>Bir hafta süren eğitim programında, yeni nesil hidrolik ve elektronik direksiyon sistemlerinin montaj, bakım ve arıza teşhis prosedürleri ele alındı.</p>",
                ContentEn = "<p>The Gümaş Automotive technical team participated in advanced steering systems training held at the TRIDEC factory in the Netherlands.</p><p>During the week-long training program, the installation, maintenance and troubleshooting procedures of new generation hydraulic and electronic steering systems were covered.</p>",
                ImageUrl = "/images/news/tridec-training.jpg",
                ThumbnailUrl = "/images/news/tridec-training-thumb.jpg",
                PublishDate = DateTime.Now.AddDays(-20),
                IsFeatured = true,
                ViewCount = 189
            },
            new()
            {
                NameTr = "Yeni Depo Tesisimiz Hizmete Girdi",
                NameEn = "Our New Warehouse Facility is Now Operational",
                Slug = "yeni-depo-tesisi-hizmete-girdi",
                SummaryTr = "5.000 m² kapalı alana sahip yeni depo tesisimiz ile stok kapasitemizi iki katına çıkardık.",
                SummaryEn = "We have doubled our stock capacity with our new warehouse facility with 5,000 m² of covered area.",
                ContentTr = "<p>Artan müşteri taleplerine daha hızlı yanıt verebilmek amacıyla İstanbul'da açtığımız yeni depo tesisimiz hizmete girdi.</p><p>Modern raf sistemleri ve otomatik envanter yönetimi ile donatılan tesisimiz, 10.000'den fazla ürün çeşidini stokta tutma kapasitesine sahip.</p>",
                ContentEn = "<p>Our new warehouse facility, which we opened in Istanbul to respond to increasing customer demands faster, has started operations.</p><p>Equipped with modern shelving systems and automatic inventory management, our facility has the capacity to keep more than 10,000 product varieties in stock.</p>",
                ImageUrl = "/images/news/new-warehouse.jpg",
                ThumbnailUrl = "/images/news/new-warehouse-thumb.jpg",
                PublishDate = DateTime.Now.AddDays(-30),
                IsFeatured = false,
                ViewCount = 312
            }
        };

        await context.News.AddRangeAsync(news);
        await context.SaveChangesAsync();
    }

    private static async Task EnsureBrandsAsync(GumasDbContext context)
    {
        if (!context.Brands.Any(b => b.Slug == "quicke"))
        {
            context.Brands.Add(new Brand
            {
                NameTr = "QUICKE",
                NameEn = "QUICKE",
                Slug = "quicke",
                DescriptionTr = "Quicke, yetmiş yılı aşkın süredir ön yükleyiciler ve ekipmanlarda ustalaşmış, tarım makineleri için akıllı ve dayanıklı çözümler sunan lider markadır.",
                DescriptionEn = "Quicke is a leading brand in front loaders and implements, offering intelligent and durable solutions for agricultural machinery for over seventy years.",
                LogoUrl = "/images/quicke-logo.jpg",
                Website = "https://www.jost-world.com/en/products/quicke.html",
                DisplayOrder = 5,
                IsFeatured = true
            });
            await context.SaveChangesAsync();
        }

        await ApplyBrandRevisionsAsync(context);
    }

    // 2.revize.pdf (19.04.2026) + 3.revize.pdf — mevcut kayıtlar üzerinde marka URL / logo / açıklama düzeltmeleri.
    // Idempotent: her çalıştırıldığında beklenen değere çeker, değişmemişse no-op.
    private static async Task ApplyBrandRevisionsAsync(GumasDbContext context)
    {
        // 3.revize.pdf Madde 1 & 4: EN açıklamaları jost-world.com'daki resmi metinlerle değiştirildi.
        const string jostDescEn = "JOST: the strong core brand. Pioneer for cast steel fifth wheel couplings and landing gears with internal gears. As the core brand, JOST offers traditional products for truck and trailer manufacturers, such as fifth wheel couplings or landing gears, as well as container equipment and axle systems.";
        const string jostLongEn = "JOST submitted the patent application for the first cast steel fifth wheel coupling over 60 years ago. Since then, JOST's fifth wheel couplings have been a success story around the world. All renowned vehicle manufacturers place their trust in JOST's expertise and performance. The extensive product range and high quality are just as crucial for this customer trust as the company's innovative skill and reliable global supply of finished and spare parts. JOST has also enjoyed continuous growth in the container equipment and interchangeable systems sector since 1990. In January 2015, JOST acquired Mercedes-Benz TrailerAxleSystems, making JOST one of Europe's important manufacturers of trailer axles.";

        const string rockingerDescEn = "ROCKINGER: for increased traction. The inventor of automated towing hitches. Be it standard towing hitches or sensor- and remote-controlled comfort couplings, the long-established ROCKINGER brand offers an extremely versatile, high-quality and reliable product portfolio.";
        const string rockingerLongEn = "Since its founding in 1875 by the master smith Johann Rockinger, ROCKINGER has made a significant impact on the technological development of towing hitches; their top level of quality has made them the brand name of choice in this sector. ROCKINGER's product range for road traffic comprises all products which are installed on tractors. In 1973, ROCKINGER expanded its product range by adding towing hitches for agriculture and forestry. In 2004, JOST took over the long-established company Regensburger Zuggabel, whose drawbars are now sold under the ROCKINGER brand.";

        const string tridecDescEn = "TRIDEC: steering systems and axle suspensions – Reliable solutions for a wide range of applications. Efficient and maintenance-friendly technology from the developer of the first manufacturer-independent steering system worldwide.";
        const string tridecLongEn = "For many years, TRIDEC systems have been used in a vast range of applications in diverse weather conditions and terrain types. They are reliable and impress thanks to their simple and quick maintenance. There are currently over 50,000 TRIDEC systems on the road. No two truck or trailer hitches are alike. Based on its many years of experience and diverse expertise, TRIDEC is able to offer tailored solutions for every task. Modular systems allow implementation on all types of trailer. Alongside steering systems, TRIDEC also manufactures independent axle suspensions for double-deck and glass-transport trailers.";

        const string quickeDescEn = "Quicke – the innovator for modern agricultural technology. Front loaders and implements for more efficiency and comfort in agriculture. Smarter farming through digitally integrated front loaders and implements for the professional.";
        const string quickeLongEn = "Quicke has been developing and producing high-quality agricultural front loaders for tractors and a wide range of implements for front loaders and attachment consoles since 1949. The world is changing, and so is farming. Quicke drives this change by making the farmer behind the machine more efficient – through superior quality and smart details built into the products. For more than seven decades Quicke has been perfecting its flagship front loaders. Buckets, silage implements or pallet – all Quicke implements are characterised by strength, quality and durability.";

        var revisions = new (string Slug, string Website, string? LogoUrl, string? DescTr, string? DescEn, string? LongEn)[]
        {
            ("jost",      "https://www.jost-world.com/en/products/jost.html",              null,                        "JOST, beşinci teker tablaları, mekanik ayaklar, kingpimler, hubodometreler, döner tablalar, konteyner kilitleri ile konteyner ekipmanları ve aks sistemlerinde önde gelen global üreticidir. Alman mühendisliği kalitesiyle üretim yapmaktadır.",                                                      jostDescEn,      jostLongEn),
            ("tridec",    "https://www.jost-world.com/en/products/tridec.html",            null,
                          "TRIDEC, treyler dümenleme ve süspansiyon sistemleri.",
                          tridecDescEn,
                          tridecLongEn),
            ("rockinger", "https://www.jost-world.com/en/products/rockinger.html",         null,                        null,                                                      rockingerDescEn, rockingerLongEn),
            ("quicke",    "https://www.jost-world.com/en/products/quicke.html",            "/images/quicke-logo.jpg",   null,                                                      quickeDescEn,    quickeLongEn),
            ("sirit",     "https://www.sirit.it/eng/catalogo_generale_sirit.pdf",          null,                        "SIRIT, ticari araçlar için üretilen en önde gelen hava freni bağlantı ve rakor markalarından biridir.",  "SIRIT today is amongst the leading brands of Air Brake Fittings dedicated to Commercial Vehicles.",  null),
            ("gerflor",   "https://www.gerflor.com",                                       null,
                          "GERFLOR, TARABUS markasıyla belediye ve şehirlerarası otobüsler için tasarlanmış ve TRAVELLER EVOLUTION markasıyla da raylı sistemler için tasarlanmış pazar lideri taban döşemesi imalatçısıdır.",
                          "GERFLOR is the market-leading manufacturer of floor coverings specifically designed for buses and coaches under the TARABUS brand and for rail systems under the TRAVELLER EVOLUTION brand.",
                          null),
        };

        bool dirty = false;
        foreach (var r in revisions)
        {
            var brand = context.Brands.FirstOrDefault(b => b.Slug == r.Slug);
            if (brand == null) continue;

            if (brand.Website != r.Website) { brand.Website = r.Website; dirty = true; }
            if (r.LogoUrl != null && brand.LogoUrl != r.LogoUrl) { brand.LogoUrl = r.LogoUrl; dirty = true; }
            if (r.DescTr != null && brand.DescriptionTr != r.DescTr) { brand.DescriptionTr = r.DescTr; dirty = true; }
            if (r.DescEn != null && brand.DescriptionEn != r.DescEn) { brand.DescriptionEn = r.DescEn; dirty = true; }
            if (r.LongEn != null && brand.LongDescriptionEn != r.LongEn) { brand.LongDescriptionEn = r.LongEn; dirty = true; }
        }

        if (dirty) await context.SaveChangesAsync();
    }

    private static async Task SeedBrandsAsync(GumasDbContext context)
    {
        var brands = new List<Brand>
        {
            new()
            {
                NameTr = "JOST",
                NameEn = "JOST",
                Slug = "jost",
                DescriptionTr = "JOST, beşinci teker tablaları, mekanik ayaklar, kingpimler, hubodometreler, döner tablalar, konteyner kilitleri ile konteyner ekipmanları ve aks sistemlerinde önde gelen global üreticidir. Alman mühendisliği kalitesiyle üretim yapmaktadır.",
                DescriptionEn = "JOST is a global leader in fifth wheels, landing gear, and container fastening systems. Manufactured with German engineering quality.",
                LogoUrl = "/images/jost-grey-logo.svg",
                Website = "https://www.jost-world.com/en/products/jost.html",
                DisplayOrder = 1,
                IsFeatured = true
            },
            new()
            {
                NameTr = "TRIDEC",
                NameEn = "TRIDEC",
                Slug = "tridec",
                DescriptionTr = "TRIDEC, treyler dümenleme ve süspansiyon sistemleri.",
                DescriptionEn = "TRIDEC, trailer steering and suspension systems.",
                LogoUrl = "/images/tridec-grey-logo.svg",
                Website = "https://www.jost-world.com/en/products/tridec.html",
                DisplayOrder = 2,
                IsFeatured = true
            },
            new()
            {
                NameTr = "Rockinger",
                NameEn = "Rockinger",
                Slug = "rockinger",
                DescriptionTr = "Rockinger, çeki sistemleri ve bağlantı elemanlarında 100 yılı aşkın deneyime sahip Alman markasıdır.",
                DescriptionEn = "Rockinger is a German brand with over 100 years of experience in towing systems and coupling components.",
                LogoUrl = "/images/rockinger-grey-logo.svg",
                Website = "https://www.jost-world.com/en/products/rockinger.html",
                DisplayOrder = 3,
                IsFeatured = true
            },
            new()
            {
                NameTr = "Gerflor",
                NameEn = "Gerflor",
                Slug = "gerflor",
                DescriptionTr = "GERFLOR, TARABUS markasıyla belediye ve şehirlerarası otobüsler için tasarlanmış ve TRAVELLER EVOLUTION markasıyla da raylı sistemler için tasarlanmış pazar lideri taban döşemesi imalatçısıdır.",
                DescriptionEn = "GERFLOR is the market-leading manufacturer of floor coverings specifically designed for buses and coaches under the TARABUS brand and for rail systems under the TRAVELLER EVOLUTION brand.",
                LogoUrl = "/images/gerflor-grey-logo.svg",
                Website = "https://www.gerflor.com",
                DisplayOrder = 4,
                IsFeatured = true
            },
            new()
            {
                NameTr = "QUICKE",
                NameEn = "QUICKE",
                Slug = "quicke",
                DescriptionTr = "Quicke, yetmiş yılı aşkın süredir ön yükleyiciler ve ekipmanlarda ustalaşmış, tarım makineleri için akıllı ve dayanıklı çözümler sunan lider markadır.",
                DescriptionEn = "Quicke is a leading brand in front loaders and implements, offering intelligent and durable solutions for agricultural machinery for over seventy years.",
                LogoUrl = "/images/quicke-logo.jpg",
                Website = "https://www.jost-world.com/en/products/quicke.html",
                DisplayOrder = 5,
                IsFeatured = true
            },
            new()
            {
                NameTr = "SIRIT",
                NameEn = "SIRIT",
                Slug = "sirit",
                DescriptionTr = "SIRIT, ticari araçlar için üretilen en önde gelen hava freni bağlantı ve rakor markalarından biridir.",
                DescriptionEn = "SIRIT today is amongst the leading brands of Air Brake Fittings dedicated to Commercial Vehicles.",
                LogoUrl = "/images/sirit-grey-logo.svg",
                Website = "https://www.sirit.it/eng/catalogo_generale_sirit.pdf",
                DisplayOrder = 6,
                IsFeatured = true
            }
        };

        await context.Brands.AddRangeAsync(brands);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCategoriesAsync(GumasDbContext context)
    {
        var categories = new List<Category>
        {
            new()
            {
                NameTr = "Aks Sistemleri",
                NameEn = "Axle Systems",
                Slug = "aks-sistemleri",
                DescriptionTr = "Ağır vasıta ve treyler için tasarlanmış yüksek performanslı aks sistemleri.",
                DescriptionEn = "High-performance axle systems designed for heavy vehicles and trailers.",
                IconClass = "bi-gear-wide-connected",
                DisplayOrder = 1
            },
            new()
            {
                NameTr = "Poyra Üniteleri",
                NameEn = "Hub Units",
                Slug = "poyra-uniteleri",
                DescriptionTr = "Araç tekerleklerinin montajı için poyra ve göbek üniteleri.",
                DescriptionEn = "Hub and wheel hub units for vehicle wheel assembly.",
                IconClass = "bi-circle",
                DisplayOrder = 2
            },
            new()
            {
                NameTr = "Kompakt Rulmanlar",
                NameEn = "Compact Bearings",
                Slug = "kompakt-rulmanlar",
                DescriptionTr = "Yüksek yük kapasiteli kompakt tekerlek rulmanları.",
                DescriptionEn = "High load capacity compact wheel bearings.",
                IconClass = "bi-bullseye",
                DisplayOrder = 3
            },
            new()
            {
                NameTr = "Hava Körük Destek Kolları",
                NameEn = "Air Spring Support Arms",
                Slug = "hava-koruk-destek-kollari",
                DescriptionTr = "Havalı süspansiyon sistemleri için destek kolları.",
                DescriptionEn = "Support arms for air suspension systems.",
                IconClass = "bi-wind",
                DisplayOrder = 4
            },
            new()
            {
                NameTr = "Makas Kulakları",
                NameEn = "Spring Hangers",
                Slug = "makas-kulaklari",
                DescriptionTr = "Yaprak yay sistemleri için makas kulakları ve bağlantı elemanları.",
                DescriptionEn = "Spring hangers and connection elements for leaf spring systems.",
                IconClass = "bi-link-45deg",
                DisplayOrder = 5
            },
            new()
            {
                NameTr = "JSK Serisi",
                NameEn = "JSK Series",
                Slug = "jsk-serisi",
                DescriptionTr = "JOST JSK serisi ürünler ve yedek parçaları.",
                DescriptionEn = "JOST JSK series products and spare parts.",
                IconClass = "bi-box-seam",
                DisplayOrder = 6
            },
            new()
            {
                NameTr = "Çeki Sistemleri",
                NameEn = "Towing Systems",
                Slug = "ceki-sistemleri",
                DescriptionTr = "Çeki kancaları, beşinci tekerlekler ve bağlantı sistemleri.",
                DescriptionEn = "Tow hooks, fifth wheels and coupling systems.",
                IconClass = "bi-link",
                DisplayOrder = 7
            },
            new()
            {
                NameTr = "Hidrolik Sistemler",
                NameEn = "Hydraulic Systems",
                Slug = "hidrolik-sistemler",
                DescriptionTr = "Damper ve kaldırma sistemleri için hidrolik silindirler.",
                DescriptionEn = "Hydraulic cylinders for tipper and lifting systems.",
                IconClass = "bi-moisture",
                DisplayOrder = 8
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(GumasDbContext context)
    {
        var brands = context.Brands.ToList();
        var categories = context.Categories.ToList();

        var jost      = brands.FirstOrDefault(b => b.Slug == "jost");
        var tridec    = brands.FirstOrDefault(b => b.Slug == "tridec");
        var rockinger = brands.FirstOrDefault(b => b.Slug == "rockinger");
        var sirit     = brands.FirstOrDefault(b => b.Slug == "sirit");
        var cekiCat   = categories.FirstOrDefault(c => c.Slug == "ceki-sistemleri");
        var aksCat    = categories.FirstOrDefault(c => c.Slug == "aks-sistemleri");

        var products = new List<Product>();

        if (jost != null && cekiCat != null)
        {
            products.Add(new()
            {
                NameTr = "JSK 37C Beşinci Teker",
                NameEn = "JSK 37C Fifth Wheel",
                Slug = "jsk-37c-besinci-teker",
                ProductCode = "JSK-37C",
                OemNumber = "JSK37CW150",
                DescriptionTr = "JOST JSK 37C serisi beşinci teker, ağır hizmet tipi çekiciler için tasarlanmıştır. 150mm bağlantı yüksekliği ile standart treyler bağlantılarına uygundur.",
                DescriptionEn = "JOST JSK 37C series fifth wheel is designed for heavy-duty tractors. With 150mm coupling height, it is suitable for standard trailer connections.",
                SpecificationsTr = "Yük Kapasitesi: 20 ton\nBağlantı Yüksekliği: 150mm\nKilitleme Tipi: Otomatik\nMalzeme: Dökme çelik",
                SpecificationsEn = "Load Capacity: 20 tons\nCoupling Height: 150mm\nLocking Type: Automatic\nMaterial: Cast steel",
                BrandId = jost.Id,
                CategoryId = cekiCat.Id,
                Status = ProductStatus.Active,
                IsFeatured = true,
                DisplayOrder = 1
            });
        }

        if (tridec != null && aksCat != null)
        {
            products.Add(new()
            {
                NameTr = "TRIDEC Direksiyon Sistemi TD-1800",
                NameEn = "TRIDEC Steering System TD-1800",
                Slug = "tridec-direksiyon-sistemi-td-1800",
                ProductCode = "TD-1800",
                OemNumber = "TRIDEC1800",
                DescriptionTr = "TRIDEC TD-1800, uzun treylerlerde mükemmel manevra kabiliyeti sağlayan hidrolik direksiyon sistemidir.",
                DescriptionEn = "TRIDEC TD-1800 is a hydraulic steering system that provides excellent maneuverability for long trailers.",
                SpecificationsTr = "Aks Sayısı: 3\nDönüş Açısı: ±25°\nKontrol: Hidrolik\nUygulanabilir Treyler Uzunluğu: 18m+",
                SpecificationsEn = "Number of Axles: 3\nTurning Angle: ±25°\nControl: Hydraulic\nApplicable Trailer Length: 18m+",
                BrandId = tridec.Id,
                CategoryId = aksCat.Id,
                Status = ProductStatus.Active,
                IsFeatured = true,
                DisplayOrder = 2
            });
        }

        if (rockinger != null && cekiCat != null)
        {
            products.Add(new()
            {
                NameTr = "Rockinger RO 400 Çeki Kancası",
                NameEn = "Rockinger RO 400 Tow Hitch",
                Slug = "rockinger-ro-400-ceki-kancasi",
                ProductCode = "RO-400",
                OemNumber = "ROCK400",
                DescriptionTr = "Rockinger RO 400 serisi çeki kancası, ağır vasıta çekicileri için tasarlanmış yüksek mukavemetli bağlantı elemanıdır.",
                DescriptionEn = "Rockinger RO 400 series tow hitch is a high-strength coupling element designed for heavy-duty tractors.",
                SpecificationsTr = "D Değeri: 150 kN\nDc Değeri: 100 kN\nV Değeri: 35 kN\nKilitleme: Otomatik",
                SpecificationsEn = "D Value: 150 kN\nDc Value: 100 kN\nV Value: 35 kN\nLocking: Automatic",
                BrandId = rockinger.Id,
                CategoryId = cekiCat.Id,
                Status = ProductStatus.Active,
                IsFeatured = true,
                DisplayOrder = 3
            });
        }

        if (sirit != null && aksCat != null)
        {
            products.Add(new()
            {
                NameTr = "SIRIT SA-12 Aks Sistemi",
                NameEn = "SIRIT SA-12 Axle System",
                Slug = "sirit-sa-12-aks-sistemi",
                ProductCode = "SA-12",
                OemNumber = "SIRIT12",
                DescriptionTr = "SIRIT SA-12 treyler aks sistemi, 12 ton kapasiteli havalı süspansiyonlu akstır.",
                DescriptionEn = "SIRIT SA-12 trailer axle system is a 12-ton capacity air suspension axle.",
                SpecificationsTr = "Kapasite: 12 ton\nSüspansiyon: Havalı\nFren: Disk\nAks Genişliği: 2550mm",
                SpecificationsEn = "Capacity: 12 tons\nSuspension: Air\nBrake: Disc\nAxle Width: 2550mm",
                BrandId = sirit.Id,
                CategoryId = aksCat.Id,
                Status = ProductStatus.Active,
                IsFeatured = false,
                DisplayOrder = 4
            });
        }

        if (products.Count > 0)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Add product images
        var savedProducts = context.Products.ToList();
        var productImages = savedProducts.Select(p => new ProductImage
        {
            ProductId = p.Id,
            ImageUrl = $"/images/products/{p.Slug}.jpg",
            ThumbnailUrl = $"/images/products/{p.Slug}-thumb.jpg",
            AltText = p.NameTr,
            IsMain = true,
            DisplayOrder = 1
        }).ToList();

        await context.ProductImages.AddRangeAsync(productImages);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSlidersAsync(GumasDbContext context)
    {
        var sliders = new List<Slider>
        {
            new()
            {
                TitleTr = "Ağır Vasıta",
                TitleEn = "Heavy Vehicle",
                SubtitleTr = "Küresel Tedarik",
                SubtitleEn = "Global Supply",
                DescriptionTr = "Ticari araçlar için yedek parça ithalatında öncü. Küresel kaliteyi yerel ihtiyaçlarla buluşturuyoruz.",
                DescriptionEn = "Pioneer in spare parts import for commercial vehicles. We combine global quality with local needs.",
                ImageUrl = "/images/slider1.jpg",
                ButtonTextTr = "Ürünleri Keşfet",
                ButtonTextEn = "Explore Products",
                ButtonUrl = "/urunler",
                DisplayOrder = 1,
                IsActive = true
            },
            new()
            {
                TitleTr = "Küresel Erişim",
                TitleEn = "Global Reach",
                SubtitleTr = "Tedarik Zinciri",
                SubtitleEn = "Supply Chain",
                DescriptionTr = "Otobüs, Kamyon ve Treyler bileşenlerinde uzmanlaşmış güçlü dağıtım ağı.",
                DescriptionEn = "Strong distribution network specialized in Bus, Truck and Trailer components.",
                ImageUrl = "/images/slider2.jpg",
                ButtonTextTr = "Markalarımız",
                ButtonTextEn = "Our Brands",
                ButtonUrl = "/markalar",
                DisplayOrder = 2,
                IsActive = true
            },
            new()
            {
                TitleTr = "Güvenilir Kalite",
                TitleEn = "Reliable Quality",
                SubtitleTr = "Premium Parçalar",
                SubtitleEn = "Premium Parts",
                DescriptionTr = "OEM standartlarında kalite garantisi ile gönül rahatlığı.",
                DescriptionEn = "Peace of mind with quality guarantee at OEM standards.",
                ImageUrl = "/images/9.jpg",
                ButtonTextTr = "İletişime Geç",
                ButtonTextEn = "Contact Us",
                ButtonUrl = "/iletisim",
                DisplayOrder = 3,
                IsActive = true
            }
        };

        await context.Sliders.AddRangeAsync(sliders);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSettingsAsync(GumasDbContext context)
    {
        var settings = new List<Setting>
        {
            new() { Key = "CompanyName", ValueTr = "Gümaş Otomotiv Sanayi ve Ticaret A.Ş.", ValueEn = "Gümaş Automotive Industry and Trade Inc.", GroupName = "General" },
            new() { Key = "CompanyAddress", ValueTr = "Taksim Cad. No:33/A 34437, Taksim - İstanbul", ValueEn = "Taksim Cad. No:33/A 34437, Taksim - Istanbul", GroupName = "Contact" },
            new() { Key = "CompanyPhone", ValueTr = "+90 212 254 78 05", ValueEn = "+90 212 254 78 05", GroupName = "Contact" },
            new() { Key = "CompanyEmail", ValueTr = "info@gumas.com.tr", ValueEn = "info@gumas.com.tr", GroupName = "Contact" },
            new() { Key = "ExperienceYears", ValueTr = "30+", ValueEn = "30+", GroupName = "Statistics" },
            new() { Key = "GlobalBrands", ValueTr = "7", ValueEn = "7", GroupName = "Statistics" },
            new() { Key = "ProductCount", ValueTr = "1000+", ValueEn = "1000+", GroupName = "Statistics" },
            new() { Key = "DealerCount", ValueTr = "500+", ValueEn = "500+", GroupName = "Statistics" },
            new() { Key = "LinkedInUrl", ValueTr = "https://www.linkedin.com/company/gümaş-a-ş/", ValueEn = "https://www.linkedin.com/company/gümaş-a-ş/", GroupName = "Social" },
            new() { Key = "WhatsAppNumber", ValueTr = "+902122547805", ValueEn = "+902122547805", GroupName = "Social" },
            new() { Key = "SiteLogoLight", ValueTr = "/images/logo/gumas_logo_light.png", ValueEn = "/images/logo/gumas_logo_light.png", GroupName = "Logos" },
            new() { Key = "SiteLogoDark", ValueTr = "/images/logo/gumas_logo_dark.png", ValueEn = "/images/logo/gumas_logo_dark.png", GroupName = "Logos" },
            new() { Key = "SiteFavicon", ValueTr = "/images/logo/gumas_logo_light.png", ValueEn = "/images/logo/gumas_logo_light.png", GroupName = "Logos" },
            new() { Key = "AdminLogo", ValueTr = "/images/logo/gumas_logo_dark.png", ValueEn = "/images/logo/gumas_logo_dark.png", GroupName = "Logos" }
        };

        await context.Settings.AddRangeAsync(settings);
        await context.SaveChangesAsync();
    }

    private static async Task EnsureLogoSettingsAsync(GumasDbContext context)
    {
        var defaultLogos = new Dictionary<string, string>
        {
            { "SiteLogoLight", "/images/logo/gumas_logo_light.png" },
            { "SiteLogoDark", "/images/logo/gumas_logo_dark.png" },
            { "SiteFavicon", "/images/logo/gumas_logo_light.png" },
            { "AdminLogo", "/images/logo/gumas_logo_dark.png" }
        };

        bool changesMade = false;
        foreach (var logo in defaultLogos)
        {
            if (!context.Settings.Any(s => s.Key == logo.Key))
            {
                context.Settings.Add(new Setting
                {
                    Key = logo.Key,
                    ValueTr = logo.Value,
                    ValueEn = logo.Value,
                    GroupName = "Logos",
                    CreatedAt = DateTime.UtcNow
                });
                changesMade = true;
            }
        }

        if (changesMade)
        {
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedTeamMembersAsync(GumasDbContext context)
    {
        var teamMembers = new List<TeamMember>
        {
            new()
            {
                NameTr = "Ahmet Yılmaz",
                NameEn = "Ahmet Yılmaz",
                TitleTr = "Genel Müdür",
                TitleEn = "General Manager",
                PhotoUrl = "/images/team/ceo.jpg",
                Email = "ahmet.yilmaz@gumas.com.tr",
                Phone = "+90 212 254 78 05",
                LinkedInUrl = "https://www.linkedin.com/company/gümaş-a-ş/",
                DisplayOrder = 1
            },
            new()
            {
                NameTr = "Mehmet Kaya",
                NameEn = "Mehmet Kaya",
                TitleTr = "Satış Müdürü",
                TitleEn = "Sales Manager",
                PhotoUrl = "/images/team/sales-manager.jpg",
                Email = "mehmet.kaya@gumas.com.tr",
                Phone = "+90 212 254 78 05",
                LinkedInUrl = "https://www.linkedin.com/company/gümaş-a-ş/",
                DisplayOrder = 2
            },
            new()
            {
                NameTr = "Fatma Demir",
                NameEn = "Fatma Demir",
                TitleTr = "Teknik Müdür",
                TitleEn = "Technical Manager",
                PhotoUrl = "/images/team/technical-manager.jpg",
                Email = "fatma.demir@gumas.com.tr",
                Phone = "+90 212 254 78 05",
                LinkedInUrl = "https://www.linkedin.com/company/gümaş-a-ş/",
                DisplayOrder = 3
            },
            new()
            {
                NameTr = "Ali Öztürk",
                NameEn = "Ali Öztürk",
                TitleTr = "Lojistik Müdürü",
                TitleEn = "Logistics Manager",
                PhotoUrl = "/images/team/logistics-manager.jpg",
                Email = "ali.ozturk@gumas.com.tr",
                Phone = "+90 212 254 78 05",
                LinkedInUrl = "https://www.linkedin.com/company/gümaş-a-ş/",
                DisplayOrder = 4
            }
        };

        await context.TeamMembers.AddRangeAsync(teamMembers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCertificatesAsync(GumasDbContext context)
    {
        var certificates = new List<Certificate>
        {
            new()
            {
                NameTr = "ISO 9001:2015 Kalite Yönetim Sistemi",
                NameEn = "ISO 9001:2015 Quality Management System",
                DescriptionTr = "Uluslararası kalite yönetim sistemi standardı sertifikası",
                DescriptionEn = "International quality management system standard certificate",
                ImageUrl = "/images/certificates/iso-9001.jpg",
                FileUrl = "/documents/iso-9001-certificate.pdf",
                IssuingOrganization = "TÜV SÜD",
                IssueDate = new DateTime(2023, 1, 15),
                ExpiryDate = new DateTime(2026, 1, 14),
                DisplayOrder = 1
            },
            new()
            {
                NameTr = "ISO 14001:2015 Çevre Yönetim Sistemi",
                NameEn = "ISO 14001:2015 Environmental Management System",
                DescriptionTr = "Çevre yönetim sistemi standardı sertifikası",
                DescriptionEn = "Environmental management system standard certificate",
                ImageUrl = "/images/certificates/iso-14001.jpg",
                FileUrl = "/documents/iso-14001-certificate.pdf",
                IssuingOrganization = "TÜV SÜD",
                IssueDate = new DateTime(2023, 3, 20),
                ExpiryDate = new DateTime(2026, 3, 19),
                DisplayOrder = 2
            },
            new()
            {
                NameTr = "JOST Yetkili Distribütör",
                NameEn = "JOST Authorized Distributor",
                DescriptionTr = "JOST ürünleri için Türkiye yetkili distribütörlük sertifikası",
                DescriptionEn = "Authorized distributor certificate for JOST products in Turkey",
                ImageUrl = "/images/certificates/jost-distributor.jpg",
                FileUrl = "/documents/jost-distributor-certificate.pdf",
                IssuingOrganization = "JOST Werke GmbH",
                IssueDate = new DateTime(2020, 6, 1),
                ExpiryDate = new DateTime(2025, 5, 31),
                DisplayOrder = 3
            }
        };

        await context.Certificates.AddRangeAsync(certificates);
        await context.SaveChangesAsync();
    }
}
