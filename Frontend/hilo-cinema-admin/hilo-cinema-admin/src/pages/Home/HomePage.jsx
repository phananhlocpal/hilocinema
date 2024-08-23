import { TableContainer, Table, Thead, Tr, Th, Td, Tbody, InputGroup } from "@chakra-ui/react";
import { Button, Input, Flex, Modal, ModalOverlay, ModalContent, ModalHeader, ModalCloseButton, ModalBody, ModalFooter } from "@chakra-ui/react";
import { FormControl, FormLabel, Textarea } from "@chakra-ui/react"
import { EditIcon, DeleteIcon } from "@chakra-ui/icons";
import { useDisclosure } from "@chakra-ui/react";
import React from "react";

const HomePage = () => {
    const { isOpen, onOpen, onClose } = useDisclosure()

    const initialRef = React.useRef(null)
    const finalRef = React.useRef(null)

    return (
        <div className="mx-5 my-5">
            <h1 className="ml-5 mb-5 font-bold text-3xl">Quản lý phim</h1>
            <div className="flex justify-between mb-5">
                <Button colorScheme='blue' className="ml-5" onClick={onOpen}>Create New</Button>
                <Input placeholder='Basic usage' style={{ width: "25rem" }} />
            </div>
            <TableContainer>
                <Table variant='simple'>
                    <Thead>
                        <Tr>
                            <Th>To convert</Th>
                            <Th>into</Th>
                            <Th isNumeric>multiply by</Th>
                            <Th isNumeric>multiply by</Th>

                        </Tr>
                    </Thead>
                    <Tbody>
                        <Tr>
                            <Td>inches</Td>
                            <Td>millimetres (mm)</Td>
                            <Td isNumeric>25.4</Td>
                            <Td>
                                <Flex justify="flex-end">
                                    <Button leftIcon={<EditIcon />} colorScheme='teal' variant='solid' mr={2}>
                                        Sửa
                                    </Button>
                                    <Button leftIcon={<DeleteIcon />} colorScheme='teal' variant='solid'>
                                        Ẩn
                                    </Button>
                                </Flex>
                            </Td>
                        </Tr>
                        <Tr>
                            <Td>feet</Td>
                            <Td>centimetres (cm)</Td>
                            <Td isNumeric>30.48</Td>
                            <Td>
                                <Flex justify="flex-end">
                                    <Button leftIcon={<EditIcon />} colorScheme='teal' variant='solid' mr={2}>
                                        Sửa
                                    </Button>
                                    <Button leftIcon={<DeleteIcon />} colorScheme='teal' variant='solid'>
                                        Ẩn
                                    </Button>
                                </Flex>
                            </Td>
                        </Tr>
                        <Tr>
                            <Td>yards</Td>
                            <Td>metres (m)</Td>
                            <Td isNumeric>0.91444</Td>
                            <Td>
                                <Flex justify="flex-end">
                                    <Button leftIcon={<EditIcon />} colorScheme='teal' variant='solid' mr={2}>
                                        Sửa
                                    </Button>
                                    <Button leftIcon={<DeleteIcon />} colorScheme='teal' variant='solid'>
                                        Ẩn
                                    </Button>
                                </Flex>
                            </Td>
                        </Tr>
                        <Tr>
                            <Td>yards</Td>
                            <Td>metres (m)</Td>
                            <Td isNumeric>0.91444</Td>
                            <Td>
                                <Flex justify="flex-end">
                                    <Button leftIcon={<EditIcon />} colorScheme='teal' variant='solid' mr={2}>
                                        Sửa
                                    </Button>
                                    <Button leftIcon={<DeleteIcon />} colorScheme='teal' variant='solid'>
                                        Ẩn
                                    </Button>
                                </Flex>
                            </Td>
                        </Tr>
                    </Tbody>
                </Table>
            </TableContainer>
            <Modal
                initialFocusRef={initialRef}
                finalFocusRef={finalRef}
                isOpen={isOpen}
                onClose={onClose}
            >
                <ModalOverlay />
                <ModalContent>
                    <ModalHeader>Create your account</ModalHeader>
                    <ModalCloseButton />
                    <ModalBody pb={6}>
                        <FormControl>
                            <FormLabel>Title</FormLabel>
                            <Input ref={initialRef} placeholder='Title' />
                        </FormControl>

                        <FormControl mt={4}>
                            <FormLabel>Duration</FormLabel>
                            <Input placeholder='Duration' />
                        </FormControl>
                        <FormControl mt={4}>
                            <FormLabel>Description</FormLabel>
                            <Textarea placeholder='Description' />
                        </FormControl>
                        <FormControl mt={4}>
                            <FormLabel>Large Image</FormLabel>
                            <InputGroup >
                                <input type={'file'} />
                            </InputGroup>
                        </FormControl>
                        <FormControl mt={4}>
                            <FormLabel>Small Image</FormLabel>
                            <InputGroup >
                                <input type={'file'} />
                            </InputGroup>
                        </FormControl>
                    </ModalBody>

                    <ModalFooter>
                        <Button colorScheme='blue' mr={3}>
                            Save
                        </Button>
                        <Button onClick={onClose}>Cancel</Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
            {/* <Modal
                initialFocusRef={initialRef}
                finalFocusRef={finalRef}
                isOpen={isOpen}
                onClose={onClose}
            >
                <ModalOverlay />
                <ModalContent>
                    <ModalHeader>Create your account</ModalHeader>
                    <ModalCloseButton />
                    <ModalBody pb={6}>
                        <FormControl>
                            <FormLabel>First name</FormLabel>
                            <Input ref={initialRef} placeholder='First name' />
                        </FormControl>

                        <FormControl mt={4}>
                            <FormLabel>Last name</FormLabel>
                            <Input placeholder='Last name' />
                        </FormControl>
                    </ModalBody>

                    <ModalFooter>
                        <Button colorScheme='blue' mr={3}>
                            Save
                        </Button>
                        <Button onClick={onClose}>Cancel</Button>
                    </ModalFooter>
                </ModalContent>
            </Modal> */}
        </div>

    );
};

export default HomePage;