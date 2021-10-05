using System;
using System.Linq;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Platform.OpenGL
{
    public class OpenGLFramebuffer : Framebuffer
    {
        private const uint MaxFramebufferSize = 8192;

        public OpenGLFramebuffer(FramebufferSpecification spec)
        {
            _specification = spec;
            foreach (var attachment in _specification.Attachments.Attachments)
            {
                if (!Utils.IsDepthFormat(attachment.TextureFormat))
                {
                    Array.Resize<>(ref _depthAttachmentSpecification, _colorAttachmentSpecifications.Length + 1);
                    _colorAttachmentSpecifications[^1] = attachment;
                }
                else
                    _depthAttachmentSpecification = attachment;
            }

            Invalidate();
        }

        ~OpenGLFramebuffer()
        {
            GL.DeleteFramebuffer(_rendererId);
            GL.DeleteTextures(_colorAttachments.ToArray()); // TODO: ToArray may be expensive
            GL.DeleteTexture(_depthAttachment);
        }


        public void Invalidate()
        {
            if (_rendererId != FramebufferHandle.Zero)
            {
                GL.DeleteFramebuffer(_rendererId);
                GL.DeleteTextures(_colorAttachments); // TODO: ToArray may be expensive
                GL.DeleteTexture(_depthAttachment);

                Array.Resize(ref _colorAttachments, 0);
                _depthAttachment = TextureHandle.Zero;
            }

            _rendererId = GL.CreateFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _rendererId);

            bool multisample = _specification.Samples > 1;

            // Attachments
            if (_colorAttachmentSpecifications.Length != 0)
            {
                Array.Resize(ref _colorAttachments, _colorAttachmentSpecifications.Length);
                var textureHandles = Utils.CreateTextures(multisample, _colorAttachments.Length);
                Array.Copy(textureHandles, _colorAttachments, _colorAttachments.Length);

                for (int i = 0; i < _colorAttachments.Length; i++)
                {
                    Utils.BindTexture(multisample, _colorAttachments[i]);
                    switch (_colorAttachmentSpecifications[i].TextureFormat)
                    {
                        case FramebufferTextureFormat.RGBA8:
                            Utils.AttachColorTexture(_colorAttachments[i], (int)_specification.Samples, InternalFormat.Rgba8, PixelFormat.Rgba, _specification.Width, _specification.Height, i);
                            break;
                        case FramebufferTextureFormat.RED_INTEGER:
                            Utils.AttachColorTexture(_colorAttachments[i], (int)_specification.Samples, InternalFormat.R32i, PixelFormat.RedInteger, _specification.Width, _specification.Height, i);
                            break;
                    }
                }
            }

            if (_depthAttachmentSpecification.TextureFormat != FramebufferTextureFormat.None)
            {
                _depthAttachment = Utils.CreateTexture(multisample);
                Utils.BindTexture(multisample, _depthAttachment);
                switch (_depthAttachmentSpecification.TextureFormat)
                {
                    case FramebufferTextureFormat.DEPTH24STENCIL8:
                        Utils.AttachDepthTexture(_depthAttachment, (int)_specification.Samples, InternalFormat.Depth24Stencil8, FramebufferAttachment.StencilAttachment, _specification.Width, _specification.Height);
                        break;
                }
            }

            if (_colorAttachments.Length > 1)
            {
                if (_colorAttachments.Length > 4)
                {
                    throw new ArgumentException("Color Attachments length was less greater than 4");
                }

                var buffers = DrawBufferMode.ColorAttachment0 | DrawBufferMode.ColorAttachment1 | DrawBufferMode.ColorAttachment2 | DrawBufferMode.ColorAttachment3;
                GL.DrawBuffers(_colorAttachments.Length, buffers);
            }
            else if (_colorAttachments.Length == 0)
            {
                // Only depth-pass
                GL.DrawBuffer(DrawBufferMode.None);
            }

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete)
            {
                throw new Exception("Framebuffer is incomplete!");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
        }

        public override void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _rendererId);
            GL.Viewport(0, 0, (int)_specification.Width, (int)_specification.Height);
        }

        public override void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
        }

        public override void Resize(uint width, uint height)
        {
            if (width == 0 || height == 0 || width > MaxFramebufferSize || height > MaxFramebufferSize)
            {
                Console.WriteLine("Warn: Attempted to rezize framebuffer to {0}, {1}", width, height);
                return;
            }
            _specification.Width = width;
            _specification.Height = height;
		
            Invalidate();
        }

        public override int ReadPixel(uint attachmentIndex, int x, int y)
        {
            if (attachmentIndex >= _colorAttachments.Length)
            {
                throw new ArgumentException("AttachmentIndex was greater than colorAttachments.Length");
            }

            GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + attachmentIndex);
            int pixelData = 0;
            GL.ReadPixels(x, y, 1, 1, PixelFormat.RedInteger, PixelType.Int, ref pixelData);
            return pixelData;
        }

        public override unsafe void ClearAttachment(uint attachmentIndex, int value)
        {
            if (attachmentIndex >= _colorAttachments.Length)
            {
                throw new ArgumentException("AttachmentIndex was greater than colorAttachments.Length");
            }

            var spec = _colorAttachmentSpecifications[attachmentIndex];
            GL.ClearTexImage(_colorAttachments[attachmentIndex], 0,
                Utils.HazelFBTextureFormatToGL(spec.TextureFormat), PixelType.Int, &value);
        }

        public override uint GetColorAttachmentRendererId(uint index = 0)
        {
            if (index >= _colorAttachments.Length)
            {
                throw new ArgumentException("AttachmentIndex was greater than colorAttachments.Length");
            }
            return (uint)_colorAttachments[index].Handle;
        }

        public override FramebufferSpecification GetSpecification()
        {
            return _specification;
        }

        private FramebufferHandle _rendererId;

        private FramebufferSpecification _specification;

        private FramebufferTextureSpecification[] _colorAttachmentSpecifications = { };
        private FramebufferTextureSpecification _depthAttachmentSpecification;

        private TextureHandle[] _colorAttachments = { };
        private TextureHandle _depthAttachment;
    }
}