import { useState, useEffect } from "react";
import axios from "axios";
import { TableContainer, Table, Thead, Tr, Th, Td, Tbody, Button, Input, Flex } from "@chakra-ui/react";
import { DeleteIcon, EditIcon } from "@chakra-ui/icons";
import CreateBlogModal from "./Create";
import UpdateBlogModal from "./Update";
import MyAlertDialog from "../../components/commom_component/alerts/AlertDialog";

const BlogPage = () => {
    const [blogs, setBlogs] = useState([]);
    const [selectedBlog, setSelectedBlog] = useState(null);
    const [isCreateOpen, setCreateOpen] = useState(false);
    const [isUpdateOpen, setUpdateOpen] = useState(false);
    const [isAlertOpen, setIsAlertOpen] = useState(false);
    const [blogToDisable, setBlogToDisable] = useState(null);
    const [searchQuery, setSearchQuery] = useState("");

    const fetchBlogs = async () => {
        const token = localStorage.getItem("jwtToken");

        try {
            const response = await axios.get("http://localhost:8000/BlogService", {
                headers: {
                    Authorization: `Bearer ${token}`,
                    Site: "admin",
                },
            });
            setBlogs(response.data);
        } catch (error) {
            console.error("Error fetching blogs:", error);
        }
    };

    const disableBlog = async (blogId) => {
        const token = localStorage.getItem("jwtToken");
    
        try {
            await axios.delete(`http://localhost:8000/BlogService/${blogId}`, null, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    Site: "admin",
                },
            });
            fetchBlogs(); 
        } catch (error) {
            console.error("Error disabling blog:", error);
        }
    };

    useEffect(() => {
        fetchBlogs();
    }, []);

    const handleCreateOpen = () => setCreateOpen(true);
    const handleCreateClose = () => setCreateOpen(false);

    const handleUpdateOpen = (blog) => {
        setSelectedBlog(blog);
        setUpdateOpen(true);
    };
    const handleUpdateClose = () => {
        setUpdateOpen(false);
        setSelectedBlog(null);
    };

    const handleAlertOpen = (blog) => {
        setBlogToDisable(blog);
        setIsAlertOpen(true);
    };

    const handleAlertClose = () => {
        setIsAlertOpen(false);
        setBlogToDisable(null);
    };

    const handleConfirmDisable = () => {
        if (blogToDisable) {
            disableBlog(blogToDisable.id);
            handleAlertClose();
        }
    };

    const filteredBlogs = blogs
        .filter((blog) => blog.title.toLowerCase().includes(searchQuery.toLowerCase()));

    return (
        <div className="mx-5 my-5">
            <h1 className="ml-5 mb-5 font-bold text-3xl">Quản lý Blogs</h1>
            <div className="flex justify-between mb-5">
                <Button
                    colorScheme="blue"
                    className="ml-5"
                    onClick={handleCreateOpen}
                    _hover={{
                        transform: "translateY(-2px)",
                        boxShadow: "lg",
                    }}
                >
                    Tạo mới
                </Button>
                <Input
                    placeholder="Tìm kiếm blog"
                    style={{ width: "25rem" }}
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                />
            </div>
            <TableContainer>
                <Table variant="simple">
                    <Thead>
                        <Tr>
                            <Th>Title</Th>
                            <Th>Description</Th>
                            <Th>Type</Th>
                            <Th isNumeric>Hành động</Th>
                        </Tr>
                    </Thead>
                    <Tbody>
                        {filteredBlogs.map((blog) => (
                            <Tr key={blog.id}>
                                <Td>{blog.title}</Td>
                                <Td>{blog.description}</Td>
                                <Td>{blog.type}</Td>
                                <Td>
                                    <Flex justify="flex-end">
                                        <Button
                                            leftIcon={<EditIcon />}
                                            colorScheme="yellow"
                                            variant="solid"
                                            mr={2}
                                            onClick={() => handleUpdateOpen(blog)}
                                        >
                                            Sửa
                                        </Button>
                                        <Button
                                            leftIcon={<DeleteIcon />}
                                            colorScheme="red"
                                            variant="solid"
                                            mr={2}
                                            onClick={() => handleAlertOpen(blog)}
                                        >
                                            Xóa
                                        </Button>
                                    </Flex>
                                </Td>
                            </Tr>
                        ))}
                    </Tbody>
                </Table>
            </TableContainer>
            <CreateBlogModal isOpen={isCreateOpen} onClose={handleCreateClose} fetchBlogs={fetchBlogs} />
            {selectedBlog && (
                <UpdateBlogModal
                    blog={selectedBlog}
                    isOpen={isUpdateOpen}
                    onClose={handleUpdateClose}
                    fetchBlogs={fetchBlogs}
                />
            )}
            <MyAlertDialog
                title="Xóa blog"
                content={`Bạn có chắc chắn muốn xóa blog này chứ "${blogToDisable?.name}" không?`}
                isOpen={isAlertOpen}
                onClose={handleAlertClose}
                onConfirm={handleConfirmDisable}
            />
        </div>
    );
};

export default BlogPage;
