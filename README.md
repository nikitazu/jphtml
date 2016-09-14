# jphtml
Japanese text analyzer with html output

# TODO
* Allow to setup which parts of speech to highlight
* Allow to setup which Kanji are names
* Try to use alternatives to MeCab as a backend
* Support PDF output format

# Building from source

## Mac OS X

```sh
git clone git@github.com:nikitazu/jphtml.git
cd jphtml
./osx.paket.install
./osx.make test
```

## Windows

```bat
git clone git@github.com:nikitazu/jphtml.git
cd jphtml
.\win.paket.install
.\win.make test
```

# 3rd party dependencies

### JMDict (included)
* Creative Commons Attribution-ShareAlike Licence (V3.0)
* http://www.edrdg.org/edrdg/licence.html

### NMeCab (via manual copying)
* GPLv2
* Original project https://osdn.jp/projects/nmecab/
* Fork with crossplatform build https://github.com/nikitazu/NMeCab.Crossplatform

### ePub Factory PCL (via nuget)
* Ms-PL
* https://epubfactory.codeplex.com/
* https://www.nuget.org/packages/EPubFactory/
