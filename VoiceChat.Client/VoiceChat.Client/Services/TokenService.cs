using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Client.Services
{
    public class TokenService
    {

        private string Token { get; set; }

        public void WriteNewToken(string token)
        {
            Token = token;
        }

        public string ReadToken() {
            return Token;
        }

    }
}
