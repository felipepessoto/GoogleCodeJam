namespace Algoritmos.AlwaysTurnLeft
{
    public class SalaQuadrada
    {
        private bool? esquerdaAberta;
        private bool? direitaAberta;
        private bool? acimaAberta;
        private bool? abaixoAberta;

        public bool? EsquerdaAberta
        {
            get { return esquerdaAberta; }
            set
            {
                if (!esquerdaAberta.HasValue)
                    esquerdaAberta = value;
            }
        }


        public bool? DireitaAberta
        {
            get { return direitaAberta; }
            set
            {
                if (!direitaAberta.HasValue)
                    direitaAberta = value;
            }
        }


        public bool? AcimaAberta
        {
            get { return acimaAberta; }
            set {
                if (!acimaAberta.HasValue)
                    acimaAberta = value;
            }
        }


        public bool? AbaixoAberta
        {
            get { return abaixoAberta; }
            set
            {
                if (!abaixoAberta.HasValue)
                    abaixoAberta = value;
            }
        }
    }
}