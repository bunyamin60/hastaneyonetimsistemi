# hastaneyonetimsistemi

--
-- PostgreSQL database dump
--

-- Dumped from database version 15.10
-- Dumped by pg_dump version 16.0

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: hastane; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA hastane;


ALTER SCHEMA hastane OWNER TO postgres;

--
-- Name: check_yonetici_departman(); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.check_yonetici_departman() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- DepartmanID için kontrol
    IF EXISTS (
        SELECT 1 
        FROM hastane.yoneticiler 
        WHERE departmanid = NEW.departmanid
    ) THEN
        -- Hata fırlat
        RAISE EXCEPTION 'Bu departman zaten dolu. Aynı departmana birden fazla yönetici eklenemez.';
    END IF;

    RETURN NEW;
END;
$$;


ALTER FUNCTION hastane.check_yonetici_departman() OWNER TO postgres;

--
-- Name: departmanadibuyukharfecevir(); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.departmanadibuyukharfecevir() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Departman adı büyük harfe dönüştürülüyor
    NEW.departmanadi := UPPER(NEW.departmanadi);
    RETURN NEW;
END;
$$;


ALTER FUNCTION hastane.departmanadibuyukharfecevir() OWNER TO postgres;

--
-- Name: guncelle_laboratuvar_test(integer, character varying, date); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.guncelle_laboratuvar_test(p_testid integer, p_testsonucu character varying, p_tarih date) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- TestID'nin var olup olmadığını kontrol et
    IF NOT EXISTS (SELECT 1 FROM hastane.laboratuvartestleri WHERE testid = p_testid) THEN
        RAISE EXCEPTION 'Geçersiz TestID: %. Güncellenecek kayıt bulunamadı.', p_testid;
    END IF;

    -- Güncelleme işlemi
    UPDATE hastane.laboratuvartestleri
    SET testsonucu = p_testsonucu,
        tarih = p_tarih
    WHERE testid = p_testid;

    -- Bilgilendirme
    RAISE NOTICE 'TestID % başarıyla güncellendi.', p_testid;
END;
$$;


ALTER FUNCTION hastane.guncelle_laboratuvar_test(p_testid integer, p_testsonucu character varying, p_tarih date) OWNER TO postgres;

--
-- Name: hastareceteekle(integer, integer, integer, integer); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.hastareceteekle(p_hastaid integer, p_doktorid integer, p_ilacid integer, p_miktar integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_ReceteID INTEGER;
BEGIN
    -- Geçerli HastaID kontrolü
    IF NOT EXISTS (SELECT 1 FROM hastane.hastalar WHERE KullaniciID = p_HastaID) THEN
        RAISE EXCEPTION 'Geçersiz HastaID: %. Lütfen geçerli bir HastaID giriniz.', p_HastaID;
    END IF;

    -- Geçerli DoktorID kontrolü
    IF NOT EXISTS (SELECT 1 FROM hastane.doktorlar WHERE KullaniciID = p_DoktorID) THEN
        RAISE EXCEPTION 'Geçersiz DoktorID: %. Lütfen geçerli bir DoktorID giriniz.', p_DoktorID;
    END IF;

    -- Geçerli IlacID kontrolü
    IF NOT EXISTS (SELECT 1 FROM hastane.ilaclar WHERE ilacid = p_IlacID) THEN
        RAISE EXCEPTION 'Geçersiz IlacID: %. Lütfen geçerli bir IlacID giriniz.', p_IlacID;
    END IF;

    -- Yeni reçete oluşturuluyor
    INSERT INTO hastane.receteler (hastaid, doktorid, tarih)
    VALUES (p_HastaID, p_DoktorID, CURRENT_DATE)
    RETURNING receteid INTO v_ReceteID;

    -- Oluşturulan reçeteye ilaç ekleniyor
    INSERT INTO hastane.receteilac (receteid, ilacid, miktar)
    VALUES (v_ReceteID, p_IlacID, p_Miktar);
END;
$$;


ALTER FUNCTION hastane.hastareceteekle(p_hastaid integer, p_doktorid integer, p_ilacid integer, p_miktar integer) OWNER TO postgres;

--
-- Name: hastarecetegecmisi(integer); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.hastarecetegecmisi(p_hastaid integer) RETURNS TABLE(receteid integer, recetetarihi date, hastaadi character varying, doktoradi character varying, ilacadi character varying, miktar integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- HastaID'nin geçerli olup olmadığını kontrol et
    IF NOT EXISTS (SELECT 1 FROM hastane.hastalar WHERE KullaniciID = p_HastaID) THEN
        RAISE EXCEPTION 'Geçersiz HastaID: %. Lütfen geçerli bir HastaID giriniz.', p_HastaID;
    END IF;

    -- Eğer hasta geçerliyse reçeteleri döndür
    RETURN QUERY
    SELECT 
        r.receteid,
        r.tarih AS recetetarihi,
        k.adsoyad AS hastaadi,
        d.adsoyad AS doktoradi,
        i.ilacadi,
        ri.miktar
    FROM hastane.receteler r
    JOIN hastane.receteilac ri ON r.receteid = ri.receteid
    JOIN hastane.ilaclar i ON ri.ilacid = i.ilacid
    JOIN hastane.kullanicilar d ON r.doktorid = d.kullaniciid
    JOIN hastane.hastalar h ON r.hastaid = h.kullaniciid
    JOIN hastane.kullanicilar k ON h.kullaniciid = k.kullaniciid
    WHERE r.hastaid = p_HastaID;
END;
$$;


ALTER FUNCTION hastane.hastarecetegecmisi(p_hastaid integer) OWNER TO postgres;

--
-- Name: kullaniciara(character varying); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.kullaniciara(aramametni character varying) RETURNS TABLE(kullaniciid integer, adsoyad character varying, telefon character varying, rol character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT k.kullaniciid, k.adsoyad, k.telefon, k.rol
    FROM hastane.kullanicilar AS k
    WHERE k.adsoyad ILIKE '%' || aramametni || '%'
       OR k.rol ILIKE '%' || aramametni || '%';
END;
$$;


ALTER FUNCTION hastane.kullaniciara(aramametni character varying) OWNER TO postgres;

--
-- Name: kullanicisil(integer); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.kullanicisil(p_kullaniciid integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Geçerli KullaniciID kontrolü
    IF NOT EXISTS (SELECT 1 FROM hastane.kullanicilar WHERE kullaniciid = p_kullaniciid) THEN
        RAISE EXCEPTION 'Geçersiz KullaniciID: %. Kullanıcı bulunamadı.', p_kullaniciid;
    END IF;

    -- Kullanıcıyı sil
    DELETE FROM hastane.kullanicilar WHERE kullaniciid = p_kullaniciid;

    -- Bilgilendirme
    RAISE NOTICE 'Kullanıcı başarıyla silindi: %', p_kullaniciid;
END;
$$;


ALTER FUNCTION hastane.kullanicisil(p_kullaniciid integer) OWNER TO postgres;

--
-- Name: telefonformatkontrol(); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.telefonformatkontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $_$
BEGIN
    -- Telefon numarasının yalnızca rakamlardan oluşup 05 ile başlamasını ve uzunluğunu kontrol et
    IF NEW.telefon !~ '^05\d{9}$' THEN
        RAISE EXCEPTION 'Telefon numarası geçerli bir formatta olmalıdır (05 ile başlamalı ve 11 rakamdan oluşmalıdır). Örnek: 05001234567';
    END IF;

    RETURN NEW;
END;
$_$;


ALTER FUNCTION hastane.telefonformatkontrol() OWNER TO postgres;

--
-- Name: testsonucukontrol(); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.testsonucukontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Test sonucun boş olmamalıdır
    IF NEW.testsonucu IS NULL OR NEW.testsonucu = '' THEN
        RAISE EXCEPTION 'Test sonuçu boş olamaz!';
    END IF;

    RETURN NEW;
END;
$$;


ALTER FUNCTION hastane.testsonucukontrol() OWNER TO postgres;

--
-- Name: yenikullaniciekle(character varying, character varying, character varying, character varying, character varying, character varying); Type: FUNCTION; Schema: hastane; Owner: postgres
--

CREATE FUNCTION hastane.yenikullaniciekle(adsoyad character varying, telefon character varying, sifre character varying, rol character varying, ekbilgi1 character varying DEFAULT NULL::character varying, ekbilgi2 character varying DEFAULT NULL::character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    yenikullaniciid INTEGER;
    temizlenmis_rol VARCHAR;
BEGIN
    -- Rol değerini temizle ve INITCAP formatına çevir
    temizlenmis_rol := INITCAP(rol);

    -- Rol kontrolü (büyük/küçük harfe duyarsız)
    IF LOWER(temizlenmis_rol) NOT IN ('yonetici', 'doktor', 'hemşire', 'hasta') THEN
        RAISE EXCEPTION 'Geçersiz Rol: %. Kabul edilen roller: Yonetici, Doktor, Hemsire, Hasta', rol;
    END IF;

    -- Kullanıcı ekleme
    INSERT INTO hastane.kullanicilar (adsoyad, telefon, sifre, rol)
    VALUES (adsoyad, telefon, sifre, temizlenmis_rol)
    RETURNING kullaniciid INTO yenikullaniciid;

    -- Çalışan Kontrolü: Çalışan tablosuna ekle
    IF LOWER(temizlenmis_rol) IN ('yonetici', 'doktor', 'hemşire') THEN
        IF ekbilgi1 IS NULL THEN
            RAISE EXCEPTION 'Çalışanlar için maaş bilgisi gereklidir.';
        END IF;
        INSERT INTO hastane.calisanlar (kullaniciid, maas)
        VALUES (yenikullaniciid, ekbilgi1::DECIMAL);
    END IF;

    -- Yönetici ekle
    IF LOWER(temizlenmis_rol) = 'yonetici' THEN
        IF ekbilgi2 IS NULL THEN
            RAISE EXCEPTION 'Yöneticiler için DepartmanID gereklidir.';
        END IF;
        -- DepartmanID kontrolü
        IF NOT EXISTS (SELECT 1 FROM hastane.departmanlar WHERE departmanid = ekbilgi2::INTEGER) THEN
            RAISE EXCEPTION 'Geçersiz DepartmanID: %. Lütfen geçerli bir DepartmanID giriniz.', ekbilgi2;
        END IF;
        INSERT INTO hastane.yoneticiler (kullaniciid, departmanid)
        VALUES (yenikullaniciid, ekbilgi2::INTEGER);

    -- Doktor ekle
    ELSIF LOWER(temizlenmis_rol) = 'doktor' THEN
        IF ekbilgi2 IS NULL THEN
            RAISE EXCEPTION 'Doktorlar için UzmanlikAlanID gereklidir.';
        END IF;
        -- UzmanlıkAlanID kontrolü
        IF NOT EXISTS (SELECT 1 FROM hastane.uzmanlikalanlari WHERE uzmanlikalanid = ekbilgi2::INTEGER) THEN
            RAISE EXCEPTION 'Geçersiz UzmanlikAlanID: %. Lütfen geçerli bir UzmanlikAlanID giriniz.', ekbilgi2;
        END IF;
        INSERT INTO hastane.doktorlar (kullaniciid, uzmanlikalanid)
        VALUES (yenikullaniciid, ekbilgi2::INTEGER);

    -- Hemşire ekle
    ELSIF LOWER(temizlenmis_rol) = 'hemşire' THEN
        IF ekbilgi2 IS NULL THEN
            RAISE EXCEPTION 'Hemşireler için ServisID gereklidir.';
        END IF;
        -- ServisID kontrolü
        IF NOT EXISTS (SELECT 1 FROM hastane.servisler WHERE servisid = ekbilgi2::INTEGER) THEN
            RAISE EXCEPTION 'Geçersiz ServisID: %. Lütfen geçerli bir ServisID giriniz.', ekbilgi2;
        END IF;
        INSERT INTO hastane.hemsireler (kullaniciid, servisid)
        VALUES (yenikullaniciid, ekbilgi2::INTEGER);

    -- Hasta ekle
    ELSIF LOWER(temizlenmis_rol) = 'hasta' THEN
        INSERT INTO hastane.hastalar (kullaniciid, dogumtarihi, adres)
        VALUES (yenikullaniciid, TO_DATE(ekbilgi1, 'YYYY-MM-DD'), ekbilgi2);
    END IF;

END;
$$;


ALTER FUNCTION hastane.yenikullaniciekle(adsoyad character varying, telefon character varying, sifre character varying, rol character varying, ekbilgi1 character varying, ekbilgi2 character varying) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: calisanlar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.calisanlar (
    kullaniciid integer NOT NULL,
    maas numeric(10,2) NOT NULL
);


ALTER TABLE hastane.calisanlar OWNER TO postgres;

--
-- Name: departmanlar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.departmanlar (
    departmanid integer NOT NULL,
    departmanadi character varying(100) NOT NULL,
    aciklama character varying(255)
);


ALTER TABLE hastane.departmanlar OWNER TO postgres;

--
-- Name: departmanlar_departmanid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane.departmanlar_departmanid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane.departmanlar_departmanid_seq OWNER TO postgres;

--
-- Name: departmanlar_departmanid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane.departmanlar_departmanid_seq OWNED BY hastane.departmanlar.departmanid;


--
-- Name: doktorlar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.doktorlar (
    kullaniciid integer NOT NULL,
    uzmanlikalanid integer NOT NULL
);


ALTER TABLE hastane.doktorlar OWNER TO postgres;

--
-- Name: hastalar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.hastalar (
    kullaniciid integer NOT NULL,
    dogumtarihi date NOT NULL,
    adres character varying(200)
);


ALTER TABLE hastane.hastalar OWNER TO postgres;

--
-- Name: hastalikgecmisi; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.hastalikgecmisi (
    gecmisid integer NOT NULL,
    hastalikid integer NOT NULL,
    hastaid integer NOT NULL,
    baslangictarihi date NOT NULL,
    bitistarihi date
);


ALTER TABLE hastane.hastalikgecmisi OWNER TO postgres;

--
-- Name: hastaliklar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.hastaliklar (
    hastalikid integer NOT NULL,
    hastalikadi character varying(100) NOT NULL
);


ALTER TABLE hastane.hastaliklar OWNER TO postgres;

--
-- Name: hastalıkgeçmişi_gecmisid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."hastalıkgeçmişi_gecmisid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."hastalıkgeçmişi_gecmisid_seq" OWNER TO postgres;

--
-- Name: hastalıkgeçmişi_gecmisid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."hastalıkgeçmişi_gecmisid_seq" OWNED BY hastane.hastalikgecmisi.gecmisid;


--
-- Name: hastalıklar_hastalikid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."hastalıklar_hastalikid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."hastalıklar_hastalikid_seq" OWNER TO postgres;

--
-- Name: hastalıklar_hastalikid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."hastalıklar_hastalikid_seq" OWNED BY hastane.hastaliklar.hastalikid;


--
-- Name: hemsireler; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.hemsireler (
    kullaniciid integer NOT NULL,
    servisid integer NOT NULL
);


ALTER TABLE hastane.hemsireler OWNER TO postgres;

--
-- Name: ilaclar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.ilaclar (
    ilacid integer NOT NULL,
    ilacadi character varying(100) NOT NULL,
    uretici character varying(100)
);


ALTER TABLE hastane.ilaclar OWNER TO postgres;

--
-- Name: kullanicilar; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.kullanicilar (
    kullaniciid integer NOT NULL,
    adsoyad character varying(100) NOT NULL,
    telefon character varying(15) NOT NULL,
    sifre character varying(50) NOT NULL,
    rol character varying(20) NOT NULL
);


ALTER TABLE hastane.kullanicilar OWNER TO postgres;

--
-- Name: kullanıcılar_kullaniciid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."kullanıcılar_kullaniciid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."kullanıcılar_kullaniciid_seq" OWNER TO postgres;

--
-- Name: kullanıcılar_kullaniciid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."kullanıcılar_kullaniciid_seq" OWNED BY hastane.kullanicilar.kullaniciid;


--
-- Name: laboratuvartestleri; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.laboratuvartestleri (
    testid integer NOT NULL,
    hastaid integer NOT NULL,
    testturuid integer NOT NULL,
    testsonucu character varying(255),
    tarih date DEFAULT CURRENT_DATE
);


ALTER TABLE hastane.laboratuvartestleri OWNER TO postgres;

--
-- Name: laboratuvartestleri_testid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane.laboratuvartestleri_testid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane.laboratuvartestleri_testid_seq OWNER TO postgres;

--
-- Name: laboratuvartestleri_testid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane.laboratuvartestleri_testid_seq OWNED BY hastane.laboratuvartestleri.testid;


--
-- Name: receteilac; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.receteilac (
    receteid integer NOT NULL,
    ilacid integer NOT NULL,
    miktar integer NOT NULL
);


ALTER TABLE hastane.receteilac OWNER TO postgres;

--
-- Name: receteler; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.receteler (
    receteid integer NOT NULL,
    hastaid integer NOT NULL,
    doktorid integer NOT NULL,
    tarih date NOT NULL
);


ALTER TABLE hastane.receteler OWNER TO postgres;

--
-- Name: reçeteler_receteid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."reçeteler_receteid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."reçeteler_receteid_seq" OWNER TO postgres;

--
-- Name: reçeteler_receteid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."reçeteler_receteid_seq" OWNED BY hastane.receteler.receteid;


--
-- Name: servisler; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.servisler (
    servisid integer NOT NULL,
    servisadi character varying(100) NOT NULL,
    kapasite integer NOT NULL,
    aciklama character varying(255)
);


ALTER TABLE hastane.servisler OWNER TO postgres;

--
-- Name: servisler_servisid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane.servisler_servisid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane.servisler_servisid_seq OWNER TO postgres;

--
-- Name: servisler_servisid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane.servisler_servisid_seq OWNED BY hastane.servisler.servisid;


--
-- Name: testturleri; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.testturleri (
    testturuid integer NOT NULL,
    testadi character varying(100) NOT NULL,
    aciklama character varying(255)
);


ALTER TABLE hastane.testturleri OWNER TO postgres;

--
-- Name: testtürleri_testturuid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."testtürleri_testturuid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."testtürleri_testturuid_seq" OWNER TO postgres;

--
-- Name: testtürleri_testturuid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."testtürleri_testturuid_seq" OWNED BY hastane.testturleri.testturuid;


--
-- Name: uzmanlikalanlari; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.uzmanlikalanlari (
    uzmanlikalanid integer NOT NULL,
    adi character varying(100) NOT NULL
);


ALTER TABLE hastane.uzmanlikalanlari OWNER TO postgres;

--
-- Name: uzmanlıkalanları_uzmanlikalanid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."uzmanlıkalanları_uzmanlikalanid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."uzmanlıkalanları_uzmanlikalanid_seq" OWNER TO postgres;

--
-- Name: uzmanlıkalanları_uzmanlikalanid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."uzmanlıkalanları_uzmanlikalanid_seq" OWNED BY hastane.uzmanlikalanlari.uzmanlikalanid;


--
-- Name: yoneticiler; Type: TABLE; Schema: hastane; Owner: postgres
--

CREATE TABLE hastane.yoneticiler (
    kullaniciid integer NOT NULL,
    departmanid integer
);


ALTER TABLE hastane.yoneticiler OWNER TO postgres;

--
-- Name: İlaçlar_ilacid_seq; Type: SEQUENCE; Schema: hastane; Owner: postgres
--

CREATE SEQUENCE hastane."İlaçlar_ilacid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hastane."İlaçlar_ilacid_seq" OWNER TO postgres;

--
-- Name: İlaçlar_ilacid_seq; Type: SEQUENCE OWNED BY; Schema: hastane; Owner: postgres
--

ALTER SEQUENCE hastane."İlaçlar_ilacid_seq" OWNED BY hastane.ilaclar.ilacid;


--
-- Name: departmanlar departmanid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.departmanlar ALTER COLUMN departmanid SET DEFAULT nextval('hastane.departmanlar_departmanid_seq'::regclass);


--
-- Name: hastalikgecmisi gecmisid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalikgecmisi ALTER COLUMN gecmisid SET DEFAULT nextval('hastane."hastalıkgeçmişi_gecmisid_seq"'::regclass);


--
-- Name: hastaliklar hastalikid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastaliklar ALTER COLUMN hastalikid SET DEFAULT nextval('hastane."hastalıklar_hastalikid_seq"'::regclass);


--
-- Name: ilaclar ilacid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.ilaclar ALTER COLUMN ilacid SET DEFAULT nextval('hastane."İlaçlar_ilacid_seq"'::regclass);


--
-- Name: kullanicilar kullaniciid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.kullanicilar ALTER COLUMN kullaniciid SET DEFAULT nextval('hastane."kullanıcılar_kullaniciid_seq"'::regclass);


--
-- Name: laboratuvartestleri testid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.laboratuvartestleri ALTER COLUMN testid SET DEFAULT nextval('hastane.laboratuvartestleri_testid_seq'::regclass);


--
-- Name: receteler receteid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteler ALTER COLUMN receteid SET DEFAULT nextval('hastane."reçeteler_receteid_seq"'::regclass);


--
-- Name: servisler servisid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.servisler ALTER COLUMN servisid SET DEFAULT nextval('hastane.servisler_servisid_seq'::regclass);


--
-- Name: testturleri testturuid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.testturleri ALTER COLUMN testturuid SET DEFAULT nextval('hastane."testtürleri_testturuid_seq"'::regclass);


--
-- Name: uzmanlikalanlari uzmanlikalanid; Type: DEFAULT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.uzmanlikalanlari ALTER COLUMN uzmanlikalanid SET DEFAULT nextval('hastane."uzmanlıkalanları_uzmanlikalanid_seq"'::regclass);


--
-- Data for Name: calisanlar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.calisanlar VALUES
	(18, 9000.00),
	(39, 60000.00),
	(40, 50000.00),
	(63, 5000.00),
	(64, 6999.00),
	(69, 10000.00);


--
-- Data for Name: departmanlar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.departmanlar VALUES
	(3, 'Bilgi Teknolojileri', 'BT Departmanı'),
	(2, 'Bilgi Heknolojileri', 'BK Departmanı'),
	(5, 'Bilgi Islem', 'BK Departmanı'),
	(1, 'MUHASEBE', 'Finans ve muhasebe işlemleri');


--
-- Data for Name: doktorlar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.doktorlar VALUES
	(18, 1),
	(39, 1),
	(40, 1),
	(63, 2),
	(64, 1);


--
-- Data for Name: hastalar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.hastalar VALUES
	(72, '2000-05-15', 'İstanbul');


--
-- Data for Name: hastalikgecmisi; Type: TABLE DATA; Schema: hastane; Owner: postgres
--



--
-- Data for Name: hastaliklar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.hastaliklar VALUES
	(1, 'grip');


--
-- Data for Name: hemsireler; Type: TABLE DATA; Schema: hastane; Owner: postgres
--



--
-- Data for Name: ilaclar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.ilaclar VALUES
	(1, 'Aspirin', 'Bayer'),
	(2, 'Parol', 'Boehringer Ingelheim'),
	(3, 'Amoxicillin', 'GlaxoSmithKline');


--
-- Data for Name: kullanicilar; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.kullanicilar VALUES
	(72, 'Ali Veli', '05551234567', 'gucluSifre123', 'Hasta'),
	(18, 'Ayşe Kaya', '05551234564', 'sifre123', 'Doktor'),
	(39, 'muhammed elzabi', '05346521232', '5665', 'Doktor'),
	(40, 'akif ', '05387653443', 'tokatlı', 'Doktor'),
	(63, 'halit', '05467655678', 'tokat', 'Doktor'),
	(64, 'alii', '05432344323', 'toakt', 'Doktor'),
	(69, 'ALİİİ', '05786533423', 'TIJTT', 'Yonetici');


--
-- Data for Name: laboratuvartestleri; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.laboratuvartestleri VALUES
	(5, 72, 1, 'bekliyor', '2024-12-23');


--
-- Data for Name: receteilac; Type: TABLE DATA; Schema: hastane; Owner: postgres
--



--
-- Data for Name: receteler; Type: TABLE DATA; Schema: hastane; Owner: postgres
--



--
-- Data for Name: servisler; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.servisler VALUES
	(1, 'yogun bakım', 50, 'kritik durumdaki hastlar icindir.'),
	(2, 'Acil', 50, 'Acil Hastalar Icindir');


--
-- Data for Name: testturleri; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.testturleri VALUES
	(1, 'kan testi', 'kandaki tüm degerler ölcülür.');


--
-- Data for Name: uzmanlikalanlari; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.uzmanlikalanlari VALUES
	(1, 'Kardiyoloji'),
	(2, 'kbb cerrahi');


--
-- Data for Name: yoneticiler; Type: TABLE DATA; Schema: hastane; Owner: postgres
--

INSERT INTO hastane.yoneticiler VALUES
	(69, 1);


--
-- Name: departmanlar_departmanid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane.departmanlar_departmanid_seq', 1, true);


--
-- Name: hastalıkgeçmişi_gecmisid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."hastalıkgeçmişi_gecmisid_seq"', 1, true);


--
-- Name: hastalıklar_hastalikid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."hastalıklar_hastalikid_seq"', 1, false);


--
-- Name: kullanıcılar_kullaniciid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."kullanıcılar_kullaniciid_seq"', 74, true);


--
-- Name: laboratuvartestleri_testid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane.laboratuvartestleri_testid_seq', 5, true);


--
-- Name: reçeteler_receteid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."reçeteler_receteid_seq"', 13, true);


--
-- Name: servisler_servisid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane.servisler_servisid_seq', 1, false);


--
-- Name: testtürleri_testturuid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."testtürleri_testturuid_seq"', 1, false);


--
-- Name: uzmanlıkalanları_uzmanlikalanid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."uzmanlıkalanları_uzmanlikalanid_seq"', 1, false);


--
-- Name: İlaçlar_ilacid_seq; Type: SEQUENCE SET; Schema: hastane; Owner: postgres
--

SELECT pg_catalog.setval('hastane."İlaçlar_ilacid_seq"', 3, true);


--
-- Name: departmanlar departmanlar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.departmanlar
    ADD CONSTRAINT departmanlar_pkey PRIMARY KEY (departmanid);


--
-- Name: doktorlar doktorlar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.doktorlar
    ADD CONSTRAINT doktorlar_pkey PRIMARY KEY (kullaniciid);


--
-- Name: hastalar hastalar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalar
    ADD CONSTRAINT hastalar_pkey PRIMARY KEY (kullaniciid);


--
-- Name: hastalikgecmisi hastalıkgeçmişi_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalikgecmisi
    ADD CONSTRAINT "hastalıkgeçmişi_pkey" PRIMARY KEY (gecmisid);


--
-- Name: hastaliklar hastalıklar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastaliklar
    ADD CONSTRAINT "hastalıklar_pkey" PRIMARY KEY (hastalikid);


--
-- Name: hemsireler hemşireler_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hemsireler
    ADD CONSTRAINT "hemşireler_pkey" PRIMARY KEY (kullaniciid);


--
-- Name: kullanicilar kullanıcılar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.kullanicilar
    ADD CONSTRAINT "kullanıcılar_pkey" PRIMARY KEY (kullaniciid);


--
-- Name: kullanicilar kullanıcılar_telefon_key; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.kullanicilar
    ADD CONSTRAINT "kullanıcılar_telefon_key" UNIQUE (telefon);


--
-- Name: laboratuvartestleri laboratuvartestleri_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.laboratuvartestleri
    ADD CONSTRAINT laboratuvartestleri_pkey PRIMARY KEY (testid);


--
-- Name: receteilac receteİlaç_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteilac
    ADD CONSTRAINT "receteİlaç_pkey" PRIMARY KEY (receteid, ilacid);


--
-- Name: receteler reçeteler_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteler
    ADD CONSTRAINT "reçeteler_pkey" PRIMARY KEY (receteid);


--
-- Name: servisler servisler_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.servisler
    ADD CONSTRAINT servisler_pkey PRIMARY KEY (servisid);


--
-- Name: testturleri testtürleri_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.testturleri
    ADD CONSTRAINT "testtürleri_pkey" PRIMARY KEY (testturuid);


--
-- Name: uzmanlikalanlari uzmanlıkalanları_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.uzmanlikalanlari
    ADD CONSTRAINT "uzmanlıkalanları_pkey" PRIMARY KEY (uzmanlikalanid);


--
-- Name: yoneticiler yöneticiler_departmanid_key; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.yoneticiler
    ADD CONSTRAINT "yöneticiler_departmanid_key" UNIQUE (departmanid);


--
-- Name: yoneticiler yöneticiler_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.yoneticiler
    ADD CONSTRAINT "yöneticiler_pkey" PRIMARY KEY (kullaniciid);


--
-- Name: calisanlar Çalışanlar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.calisanlar
    ADD CONSTRAINT "Çalışanlar_pkey" PRIMARY KEY (kullaniciid);


--
-- Name: ilaclar İlaçlar_pkey; Type: CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.ilaclar
    ADD CONSTRAINT "İlaçlar_pkey" PRIMARY KEY (ilacid);


--
-- Name: yoneticiler check_yonetici_departman_trig; Type: TRIGGER; Schema: hastane; Owner: postgres
--

CREATE TRIGGER check_yonetici_departman_trig BEFORE INSERT ON hastane.yoneticiler FOR EACH ROW EXECUTE FUNCTION hastane.check_yonetici_departman();


--
-- Name: departmanlar departmanadibuyukharfecevirtrig; Type: TRIGGER; Schema: hastane; Owner: postgres
--

CREATE TRIGGER departmanadibuyukharfecevirtrig BEFORE INSERT ON hastane.departmanlar FOR EACH ROW EXECUTE FUNCTION hastane.departmanadibuyukharfecevir();


--
-- Name: kullanicilar telefonkontroltrig; Type: TRIGGER; Schema: hastane; Owner: postgres
--

CREATE TRIGGER telefonkontroltrig BEFORE INSERT OR UPDATE ON hastane.kullanicilar FOR EACH ROW EXECUTE FUNCTION hastane.telefonformatkontrol();


--
-- Name: laboratuvartestleri testsonucukontroltrig; Type: TRIGGER; Schema: hastane; Owner: postgres
--

CREATE TRIGGER testsonucukontroltrig BEFORE INSERT ON hastane.laboratuvartestleri FOR EACH ROW EXECUTE FUNCTION hastane.testsonucukontrol();


--
-- Name: calisanlar calisanlar_kullaniciid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.calisanlar
    ADD CONSTRAINT calisanlar_kullaniciid_fkey FOREIGN KEY (kullaniciid) REFERENCES hastane.kullanicilar(kullaniciid) ON DELETE CASCADE;


--
-- Name: doktorlar doktorlar_kullaniciid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.doktorlar
    ADD CONSTRAINT doktorlar_kullaniciid_fkey FOREIGN KEY (kullaniciid) REFERENCES hastane.kullanicilar(kullaniciid) ON DELETE CASCADE;


--
-- Name: doktorlar doktorlar_uzmanlikalanid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.doktorlar
    ADD CONSTRAINT doktorlar_uzmanlikalanid_fkey FOREIGN KEY (uzmanlikalanid) REFERENCES hastane.uzmanlikalanlari(uzmanlikalanid);


--
-- Name: hastalikgecmisi fk_hasta_gecmis; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalikgecmisi
    ADD CONSTRAINT fk_hasta_gecmis FOREIGN KEY (hastaid) REFERENCES hastane.hastalar(kullaniciid) ON DELETE CASCADE;


--
-- Name: receteler fk_hasta_recete; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteler
    ADD CONSTRAINT fk_hasta_recete FOREIGN KEY (hastaid) REFERENCES hastane.hastalar(kullaniciid) ON DELETE CASCADE;


--
-- Name: hastalar hastalar_kullaniciid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalar
    ADD CONSTRAINT hastalar_kullaniciid_fkey FOREIGN KEY (kullaniciid) REFERENCES hastane.kullanicilar(kullaniciid) ON DELETE CASCADE;


--
-- Name: hastalikgecmisi hastalıkgeçmişi_hastaid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalikgecmisi
    ADD CONSTRAINT "hastalıkgeçmişi_hastaid_fkey" FOREIGN KEY (hastaid) REFERENCES hastane.hastalar(kullaniciid) ON DELETE CASCADE;


--
-- Name: hastalikgecmisi hastalıkgeçmişi_hastalikid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hastalikgecmisi
    ADD CONSTRAINT "hastalıkgeçmişi_hastalikid_fkey" FOREIGN KEY (hastalikid) REFERENCES hastane.hastaliklar(hastalikid);


--
-- Name: hemsireler hemsireler_kullaniciid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hemsireler
    ADD CONSTRAINT hemsireler_kullaniciid_fkey FOREIGN KEY (kullaniciid) REFERENCES hastane.kullanicilar(kullaniciid) ON DELETE CASCADE;


--
-- Name: hemsireler hemşireler_servisid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.hemsireler
    ADD CONSTRAINT "hemşireler_servisid_fkey" FOREIGN KEY (servisid) REFERENCES hastane.servisler(servisid);


--
-- Name: laboratuvartestleri laboratuvartestleri_hastaid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.laboratuvartestleri
    ADD CONSTRAINT laboratuvartestleri_hastaid_fkey FOREIGN KEY (hastaid) REFERENCES hastane.hastalar(kullaniciid) ON DELETE CASCADE;


--
-- Name: laboratuvartestleri laboratuvartestleri_testturuid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.laboratuvartestleri
    ADD CONSTRAINT laboratuvartestleri_testturuid_fkey FOREIGN KEY (testturuid) REFERENCES hastane.testturleri(testturuid);


--
-- Name: receteler receteler_doktorid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteler
    ADD CONSTRAINT receteler_doktorid_fkey FOREIGN KEY (doktorid) REFERENCES hastane.doktorlar(kullaniciid) ON DELETE CASCADE;


--
-- Name: receteler receteler_hastaid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteler
    ADD CONSTRAINT receteler_hastaid_fkey FOREIGN KEY (doktorid) REFERENCES hastane.doktorlar(kullaniciid) ON DELETE CASCADE;


--
-- Name: receteilac receteİlaç_ilacid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteilac
    ADD CONSTRAINT "receteİlaç_ilacid_fkey" FOREIGN KEY (ilacid) REFERENCES hastane.ilaclar(ilacid);


--
-- Name: receteilac receteİlaç_receteid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.receteilac
    ADD CONSTRAINT "receteİlaç_receteid_fkey" FOREIGN KEY (receteid) REFERENCES hastane.receteler(receteid) ON DELETE CASCADE;


--
-- Name: yoneticiler yoneticiler_kullaniciid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.yoneticiler
    ADD CONSTRAINT yoneticiler_kullaniciid_fkey FOREIGN KEY (kullaniciid) REFERENCES hastane.kullanicilar(kullaniciid) ON DELETE CASCADE;


--
-- Name: yoneticiler yöneticiler_departmanid_fkey; Type: FK CONSTRAINT; Schema: hastane; Owner: postgres
--

ALTER TABLE ONLY hastane.yoneticiler
    ADD CONSTRAINT "yöneticiler_departmanid_fkey" FOREIGN KEY (departmanid) REFERENCES hastane.departmanlar(departmanid);


--
-- PostgreSQL database dump complete
--

