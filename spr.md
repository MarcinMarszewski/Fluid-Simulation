# Symulacja SPH
SPH (Smoothed Particle Hydrodynamics) to metoda numeryczna stosowana do symulacji płynów i gazów. Została opracowana w latach 70. XX wieku i jest szeroko wykorzystywana w grafice komputerowej, astrofizyce oraz inżynierii.
W SPH płyn jest reprezentowany przez zbiór cząstek (ang. particles), z których każda przenosi informacje o masie, pozycji, prędkości i innych właściwościach fizycznych. Wartości pól fizycznych (np. gęstość, ciśnienie) są wyznaczane poprzez uśrednianie właściwości sąsiednich cząstek za pomocą funkcji wygładzającej (ang. smoothing kernel).

# Planowana implementacja
Metoda sph obsługująca jeden rodzaj cieczy z podstawowym modelem uwzględniającym siły lepkości, grawitacji i sprężystości cieczy.

Docelowa badania dotyczą stworzenia klastra obliczeniowego wykorzystującego wiele maszyn z procesorami graifcznymi.
Projekt zakłada odpowiednie zrównoleglenie w obrębie procesora graficznego (wstępny reaserch wskazuje na konieczność implementacji algorytmów sortowania z użyciem precesorów graficznych oraz podziału przestrzeni), oraz synchornizację pomiędzy maszynami.
Dzielenie pracy pomiędzy maszynami zakłada bardziej zaawansowany system podziału przestrzeni, uwzględniający fizyczne właściwości sprzętu.
Pojedyncza maszyna wykonywałaby obliczenia dla zestawu podprzestrzeni wewnętrznych wyłącznych dla niej, oraz podprzestrzeni zewnętrznych wymagających synchronizacji z innymi maszynami.

alt:  
symulacja z wyświetlaniem: pozycji, gęstości, prędkości. siatki
opcje podziału przestrzeni: równomierna siatka (różne metody numerowania), nierównomierna siatka, quad-tree
algorytmy sortowania: bitonic, radix
wprowadzanie parametrów symulacji

# Etapy projektu
## Implementacja sekwencyjna w środowisku unity
## Wykorzystanie unity compute shaders 
## Wersja wykorzystująca technologię cuda
## Wykorzystanie wielu maszyn do utworzenia klastra obliczeniowego
## Potencjalne dalsze zaawansowane techniki optymalizacyjne


# Zasoby:
https://matthias-research.github.io/pages/publications/sca03.pdf
https://www.sci.utah.edu/publications/premoze03/ParticleFluidsHiRes.pdf
https://cg.informatik.uni-freiburg.de/course_notes/sim_10_sph.pdf
https://developer.download.nvidia.com/presentations/2008/GDC/GDC08_ParticleFluids.pdf
https://www.ljll.fr/~frey/papers/levelsets/Clavet%20S.,%20Particle-based%20viscoelastic%20fluid%20simulation.pdf
https://www-cs.ccny.cuny.edu/~jzhang/papers/rtree_tr.pdf