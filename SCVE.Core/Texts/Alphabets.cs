namespace SCVE.Core.Texts
{
    public class Alphabets
    {
        /// <summary>
        /// Default alphabet with basically all common chars (EN + RU + Digits + Controls)
        /// <remarks>
        /// <para>abcdefghijklmnopqrstuvwxyz</para>
        /// <para>ABCDEFGHIJKLMNOPQRSTUVWXYZ</para>
        /// <para>абвгдеёжзийклмнопрстуфхцчшщъыьэюя</para>
        /// <para>АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ</para>
        /// <para>0123456789</para>
        /// <para>! &quot; # $ % ^ &amp; * ( ) + = - _ ' ? . , | / ` ~ № : ; @ [ ] { }</para>
        /// </remarks>
        /// </summary>
        public const string Default =
            "abcdefghijklmnopqrstuvwxyz" +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" +
            "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
            "0123456789" +
            "!\"#$%^&*()+=-_'?.,|/`~№:;@[]{}";
    }
}