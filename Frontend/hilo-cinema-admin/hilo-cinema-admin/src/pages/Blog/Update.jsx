import { useState, useEffect } from "react";
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalCloseButton, ModalBody, ModalFooter, Button, Select } from "@chakra-ui/react";
import axios from "axios";
import PropTypes from "prop-types";
import { toBase64 } from "../../utils/toBase64";

const UpdateBlogModal = ({ blog, isOpen, onClose, fetchBlogs }) => {
    const [title, setTitle] = useState(blog.title);
    const [description, setDescription] = useState(blog.description);
    const [content, setContent] = useState(blog.content);
    const [type, setType] = useState(blog.type);
    const [img, setImage] = useState('');
    const [status, setStatus] = useState(blog.status);
    const employeeId = blog.employeeId; 

    useEffect(() => {
        if (isOpen) {
            const script = document.createElement("script");
            script.src = "https://cdn.ckeditor.com/4.16.0/standard/ckeditor.js";
            script.async = true;
            script.onload = () => {
                if (window.CKEDITOR) {
                    window.CKEDITOR.replace('content');
                    window.CKEDITOR.instances.content.setData(blog.content); 
                }
            };
            document.body.appendChild(script);
        }
    }, [isOpen, blog.content]);

    const onUpdate = async () => {
        const token = localStorage.getItem("jwtToken");

        const data = {
            id: blog.id,
            title,
            description,
            content: window.CKEDITOR.instances.content.getData(),
            type,
            img: img ? await toBase64(img) : blog.img, 
            status,
            employeeId,
        };

        try {
            await axios.put(`http://localhost:8000/BlogService/${blog.id}`, data, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                    Site: 'admin',
                    Role: 'admin'
                },
            });
            onClose();
            fetchBlogs(); 
        } catch (error) {
            console.error("Error updating blog:", error.response ? error.response.data : error.message);
        }
    };

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        setImage(file);
    };

    return (
        <Modal isOpen={isOpen} onClose={onClose} size={"3xl"}>
            <ModalOverlay />
            <ModalContent>
                <ModalHeader>Update Blog Post</ModalHeader>
                <ModalCloseButton />
                <ModalBody pb={2}>
                    <div className="py-6">
                        <div className="max-w-7xl mx-auto sm:px-6 lg:px-8">
                            <div className="bg-white overflow-hidden shadow-sm sm:rounded-lg">
                                <div className="p-6 bg-white border-b border-gray-200">
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Title <span className="text-red-500">*</span></label><br />
                                        <input
                                            type="text"
                                            className="border-2 border-gray-300 p-2 w-full"
                                            name="title"
                                            id="title"
                                            value={title}
                                            onChange={(e) => setTitle(e.target.value)}
                                            required
                                        />
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Type</label><br />
                                        <Select
                                            placeholder="Select Type"
                                            value={type}
                                            onChange={(e) => setType(e.target.value)}
                                        >
                                            <option value='Blog'>Blog</option>
                                            <option value='Promotion'>Promotion</option>
                                        </Select>
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Image</label><br />
                                        <input
                                            type="file"
                                            className="border-2 border-gray-300 p-2 w-full"
                                            name="img"
                                            id="img"
                                            onChange={handleImageChange}
                                        />
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Description</label><br />
                                        <input
                                            type="text"
                                            className="border-2 border-gray-300 p-2 w-full"
                                            name="description"
                                            id="description"
                                            placeholder="(Optional)"
                                            value={description}
                                            onChange={(e) => setDescription(e.target.value)}
                                        />
                                    </div>
                                    <div className="mb-8">
                                        <label className="text-xl text-gray-600">Content <span className="text-red-500">*</span></label><br />
                                        <textarea
                                            name="content"
                                            className="border-2 border-gray-500"
                                            value={content}
                                            onChange={(e) => setContent(e.target.value)}
                                        >
                                        </textarea>
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Status</label><br />
                                        <Select
                                            placeholder="Select Status"
                                            value={status}
                                            onChange={(e) => setStatus(e.target.value)}
                                        >
                                            <option value='Active'>Active</option>
                                            <option value='Inactive'>Inactive</option>
                                        </Select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ModalBody>

                <ModalFooter>
                    <Button onClick={onUpdate}>Update</Button>
                    <Button onClick={onClose}>Cancel</Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

UpdateBlogModal.propTypes = {
    blog: PropTypes.object.isRequired,
    isOpen: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    fetchBlogs: PropTypes.func.isRequired,
};

export default UpdateBlogModal;